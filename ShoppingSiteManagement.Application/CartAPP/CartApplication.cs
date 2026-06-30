using _0_Framework.Application;
using ShoppingSiteManagement.Application.Contracts.CartAPC;
using ShoppingSiteManagement.Application.Contracts.OrderAPC;
using ShoppingSiteManagement.Application.OrderAPP;
using ShoppingSiteManagement.Domain.CartAgg;
using ShoppingSiteManagement.Domain.ProductAgg;
using ShoppingSiteManagement.Infrastructure.EFCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShoppingSiteManagement.Application.CartAPP
{
    public class CartApplication : ICartApplication
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly IOrderApplication _orderApplication;
        private readonly ShoppingSiteContext _dbContext; // جدید

        public CartApplication(
            ICartRepository cartRepository,
            IProductRepository productRepository,
            IOrderApplication orderApplication,
            ShoppingSiteContext dbContext) // جدید
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _orderApplication = orderApplication; // توجه: اسم فیلد خودت رو حفظ کن اگر فرق داره
            _dbContext = dbContext; // جدید
        }

        public OperationResult AddToCart(long productId, string accountEmail)
        {
            var operation = new OperationResult();
            var product = _productRepository.Get(productId);
            if (product == null) return operation.Failed("محصول یافت نشد.");

            var cart = _cartRepository.GetActiveCartBy(accountEmail);

            if (cart == null)
            {
                cart = new Cart(accountEmail);
                _cartRepository.Add(cart);
                _cartRepository.SaveChanges();
            }

            var existingItem = cart.Items.FirstOrDefault(x => x.ProductId == productId);
            if (existingItem != null)
            {
                if (existingItem.Count + 1 > product.StockCount)
                    return operation.Failed("موجودی انبار کافی نیست.");
                existingItem.Increase(1);
            }
            else
            {
                var price = product.HasDiscount ? product.DiscountedPrice.Value : product.Price;
                cart.Items.Add(new CartItem(cart.Id, productId, 1, price));
            }

            cart.CalculateTotalAmount();
            _cartRepository.SaveChanges();
            return operation.Success();
        }

        public OperationResult IncreaseItemCount(long itemId)
        {
            var operation = new OperationResult();
            var cart = _cartRepository.GetByItemId(itemId);
            if (cart == null) return operation.Failed("سبد خرید یافت نشد.");

            var item = cart.Items.FirstOrDefault(x => x.Id == itemId);
            var product = _productRepository.Get(item.ProductId);

            if (item.Count + 1 > product.StockCount)
                return operation.Failed("بیشتر از موجودی انبار امکان‌پذیر نیست.");

            item.Increase(1);
            cart.CalculateTotalAmount();
            _cartRepository.SaveChanges();
            return operation.Success();
        }

        public OperationResult DecreaseItemCount(long itemId)
        {
            var operation = new OperationResult();
            var cart = _cartRepository.GetByItemId(itemId);
            if (cart == null) return operation.Failed("سبد خرید یافت نشد.");

            var item = cart.Items.FirstOrDefault(x => x.Id == itemId);
            if (item.Count > 1)
            {
                item.Decrease(1);
                cart.CalculateTotalAmount();
                _cartRepository.SaveChanges();
            }
            return operation.Success();
        }

        public OperationResult RemoveFromCart(long itemId)
        {
            var operation = new OperationResult();
            var cart = _cartRepository.GetByItemId(itemId);
            var item = cart.Items.FirstOrDefault(x => x.Id == itemId);
            if (item != null)
            {
                cart.Items.Remove(item);
                cart.CalculateTotalAmount();
                _cartRepository.SaveChanges();
            }
            return operation.Success();
        }
        public OperationResult Checkout(string accountEmail)
        {
            var operation = new OperationResult();
            var cart = _cartRepository.GetActiveCartBy(accountEmail);

            if (cart == null || !cart.Items.Any())
                return operation.Failed("سبد خرید شما خالی است.");

            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {
                // 1) خواندن و بررسی موجودی
                var items = cart.Items.ToList();
                var productIds = items.Select(i => i.ProductId).Distinct().ToList();
                var products = productIds.Select(id => _productRepository.Get(id)).ToList();

                foreach (var item in items)
                {
                    var product = products.FirstOrDefault(p => p.Id == item.ProductId);
                    if (product == null)
                    {
                        transaction.Rollback();
                        return operation.Failed($"محصول یافت نشد (Id = {item.ProductId})");
                    }

                    if (product.StockCount < item.Count)
                    {
                        transaction.Rollback();
                        return operation.Failed($"موجودی کافی نیست برای {product.Name} (موجود: {product.StockCount}، درخواستی: {item.Count})");
                    }
                }

                // 2) کاهش موجودی
                foreach (var item in items)
                {
                    var product = products.First(p => p.Id == item.ProductId);
                    try
                    {
                        product.ReduceStock(item.Count);
                    }
                    catch (Exception stockEx)
                    {
                        transaction.Rollback();
                        return operation.Failed($"خطا در کاهش موجودی: {stockEx.Message}");
                    }
                }

                // 3) ساخت سفارش
                var createOrderDto = new CreateOrderFromCartDto
                {
                    AccountEmail = accountEmail,
                    TotalProductsPrice = cart.TotalAmount,
                    ShippingCost = 0,
                    Items = items.Select(item =>
                    {
                        var product = products.First(p => p.Id == item.ProductId);
                        return new CreateOrderItemDto
                        {
                            ProductId = item.ProductId,
                            ProductName = product.Name,
                            ProductImage = product.OrginalImage,
                            UnitPrice = item.UnitPrice,
                            Count = item.Count
                        };
                    }).ToList()
                };

                var createOrderResult = _orderApplication.CreateOrderFromCart(createOrderDto);
                if (!createOrderResult.IsSuccessed)
                {
                    transaction.Rollback();
                    return operation.Failed($"خطا در ایجاد سفارش: {createOrderResult.Message}");
                }

                // 4) تکمیل سبد
                cart.Finish();

                // 5) ذخیره نهایی
                try
                {
                    _cartRepository.SaveChanges();
                }
                catch (Exception saveEx)
                {
                    transaction.Rollback();
                    return operation.Failed($"خطا در ذخیره سفارش در دیتابیس: {saveEx.InnerException?.Message ?? saveEx.Message}");
                }

                // 6) commit تراکنش
                transaction.Commit();
                return operation.Success("سفارش شما با موفقیت ایجاد شد.");
            }
            catch (Exception ex)
            {
                try { transaction.Rollback(); } catch { }
                return operation.Failed($"خطای نامشخص در checkout: {ex.InnerException?.Message ?? ex.Message}");
            }
        }
        public OperationResult ReleaseReservedStock(string accountEmail)
        {
            var operation = new OperationResult();
            var cart = _cartRepository.GetByAccountEmail(accountEmail);

            if (cart != null && cart.IsFinished)
            {
                foreach (var item in cart.Items)
                {
                    var product = _productRepository.Get(item.ProductId);
                    if (product != null)
                    {
                        product.IncreaseStock(item.Count);
                    }
                }
                cart.Reopen();
                _cartRepository.SaveChanges();
            }
            return operation.Success();
        }

        public CartViewModel GetCart(string accountEmail)
        {
            var cart = _cartRepository.GetActiveCartBy(accountEmail);
            if (cart == null) return new CartViewModel { Items = new List<CartItemViewModel>() };

            return new CartViewModel
            {
                Id = cart.Id,
                TotalAmount = cart.TotalAmount,
                TotalItems = cart.Items.Sum(x => x.Count),
                Items = cart.Items.Select(item => {
                    var product = _productRepository.Get(item.ProductId);
                    int discountRate = 0;
                    if (product.HasDiscount && product.Price > 0)
                    {
                        discountRate = (int)((product.Price - product.DiscountedPrice.Value) / product.Price * 100);
                    }
                    return new CartItemViewModel
                    {
                        Id = item.Id,
                        ProductId = item.ProductId,
                        ProductName = product.Name,
                        ProductImage = product.OrginalImage,
                        UnitPrice = product.Price,
                        Count = item.Count,
                        TotalPrice = item.TotalPrice,
                        HasDiscount = product.HasDiscount,
                        DiscountedPrice = product.DiscountedPrice ?? 0,
                        DiscountRate = discountRate,
                        StockCount = product.StockCount,
                        Color = product.Color,
                        Size = product.Size
                    };
                }).Where(x => x.IsDeleted == false).ToList()
            };
        }

        public OperationResult ReleaseExpiredCarts() { return new OperationResult().Success(); }
    }
}