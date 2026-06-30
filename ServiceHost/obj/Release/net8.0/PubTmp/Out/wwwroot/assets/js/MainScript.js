if (Number(document.getElementById("badge-count").textContent < 1)) {
    document.getElementById("badge-count").style.opacity = "0";
}
else {
    document.getElementById("badge-count").style.opacity = "1";
}
document.addEventListener('DOMContentLoaded', function () {
    e.preventDefault();
    const dockItems = document.querySelectorAll('.dock-item');
    dockItems.forEach(item => {
        item.addEventListener('click', function (e) {
            dockItems.forEach(i => i.classList.remove('active'));
            this.classList.add('active');
        });
    });
    const rippleBtns = document.querySelectorAll('.ripple-btn');
    rippleBtns.forEach(btn => {
        btn.addEventListener('click', function (e) {
            const x = e.clientX - e.target.getBoundingClientRect().left;
            const y = e.clientY - e.target.getBoundingClientRect().top;
            const ripple = document.createElement('span');
            ripple.classList.add('ripple');
            ripple.style.left = x + 'px';
            ripple.style.top = y + 'px';
            this.appendChild(ripple);
            setTimeout(() => {
                ripple.remove();
            }, 600);
        });
    });

    const searchBtn = document.querySelector('.ripple-btn-search');
    if (searchBtn) {
        searchBtn.addEventListener('click', function (e) {
            const x = e.clientX - e.target.getBoundingClientRect().left;
            const y = e.clientY - e.target.getBoundingClientRect().top;
            const ripple = document.createElement('span');
            ripple.classList.add('ripple');
            ripple.style.left = x + 'px';
            ripple.style.top = y + 'px';
            this.appendChild(ripple);
            setTimeout(() => {
                ripple.remove();
            }, 600);
        });
    }
});


document.addEventListener('DOMContentLoaded', function () {
    const productsWrapper = document.getElementById('productsWrapper');
    const scrollHint = document.querySelector('.scroll-hint');

    let hintVisible = true;

    productsWrapper.addEventListener('scroll', function () {
        if (hintVisible) {
            scrollHint.style.opacity = '0';
            hintVisible = false;
        }
    });
});


// گالری
const galleryTrack = document.getElementById('galleryTrack');
const items = document.querySelectorAll('.gallery-item');
const dotsContainer = document.getElementById('galleryDots');
let current = 0;

items.forEach((item, i) => {
    const dot = document.createElement('div');
    dot.className = i === 0 ? 'dot active' : 'dot';
    dot.addEventListener('click', () => setSlide(i));
    dotsContainer.appendChild(dot);
});

function setSlide(index) {
    current = index;
    const offset = index * (items[0].offsetWidth + 15);
    galleryTrack.style.transform = `translateX(-${offset}px)`;
    items.forEach((item, i) => item.classList.toggle('active', i === index));
    document.querySelectorAll('.gallery-pagination .dot').forEach((d, i) => d.classList.toggle('active', i === index));
}

// swipe موبایل
let touchStartX = 0;
galleryTrack.addEventListener('touchstart', e => touchStartX = e.changedTouches[0].screenX);
galleryTrack.addEventListener('touchend', e => {
    let touchEndX = e.changedTouches[0].screenX;
    if (touchEndX - touchStartX > 50) setSlide(Math.max(current - 1, 0));
    if (touchStartX - touchEndX > 50) setSlide(Math.min(current + 1, items.length - 1));
});

// تعداد
const decreaseBtn = document.getElementById('decrease');
const increaseBtn = document.getElementById('increase');
const quantityInput = document.getElementById('quantity');
decreaseBtn.addEventListener('click', () => { let val = parseInt(quantityInput.value); if (val > 1) quantityInput.value = val - 1; });
increaseBtn.addEventListener('click', () => { let val = parseInt(quantityInput.value); quantityInput.value = val + 1; });

// category
const filterBtns = document.querySelectorAll('.mini-cat');
const products = document.querySelectorAll('.product-card');

filterBtns.forEach(btn => {
    btn.addEventListener('click', () => {

        // active class
        filterBtns.forEach(b => b.classList.remove('active'));
        btn.classList.add('active');

        const filter = btn.dataset.filter;

        products.forEach(card => {
            if (filter === 'all') {
                card.style.display = 'block';
            } else {
                card.style.display = card.dataset.category.includes(filter)
                    ? 'block'
                    : 'none';
            }
        });
    });
});


// مدیریت سبد خرید پارسو شاپ
const CART_KEY = "parso-cart";

function addToCart(id, name, price, picture) {
    let cart = JSON.parse(localStorage.getItem(CART_KEY)) || [];

    const existingItem = cart.find(x => x.id === id);

    if (existingItem) {
        existingItem.count += 1;
    } else {
        const item = {
            id: id,
            name: name,
            price: price,
            picture: picture,
            count: 1
        };
        cart.push(item);
    }

    localStorage.setItem(CART_KEY, JSON.stringify(cart));
    updateCartBadge();

    // نمایش نوتیفیکیشن ساده
    showToast(`${name} به سبد خرید اضافه شد`);
}

function updateCartBadge() {
    let cart = JSON.parse(localStorage.getItem(CART_KEY)) || [];
    const count = cart.reduce((sum, item) => sum + item.count, 0);
    const badge = document.getElementById("badge-count");
    if (badge) {
        badge.innerText = count;
        badge.style.display = count > 0 ? "block" : "none";
    }
}

// این تابع رو برای نمایش پیغام اضافه کن
function showToast(message) {
    // فعلا یه الرت ساده، بعدا میتونی با سوعیت الرت خوشگلش کنی
    console.log(message);
}

// لود اولیه عدد سبد خرید
document.addEventListener("DOMContentLoaded", updateCartBadge);