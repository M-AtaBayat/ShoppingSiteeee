// type loop
const text2 = "پارسو شاپ";
const typingSpeed = 70;
const deletingSpeed = 70;
const pauseAfterDelete = 4000;

const el2 = document.getElementById("typeloop");

let index = 0;
let isDeleting = false;

function typeLoop2() {
    if (!isDeleting) {
        el2.textContent = text2.slice(0, index + 1);
        index++;
        if (index === text2.length) {
            setTimeout(() => isDeleting = true, 800);
        }
    } else {
        el2.textContent = text2.slice(0, index - 1);
        index--;
        if (index === 1) {
            isDeleting = false;
            setTimeout(() => { }, pauseAfterDelete);
        }
    }
    const speed = isDeleting ? deletingSpeed : typingSpeed;
    setTimeout(typeLoop2, speed);
}
typeLoop2();