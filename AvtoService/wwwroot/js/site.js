console.log("site.js loaded!");

function initializeCarousel() {
    console.log("Initializing carousel...");
    const images = [
        '../img/bugatti.jpg',
        '../img/car2.jpg',
        '../img/carrr.jpg'
    ];

    let currentIndex = 0;

    const imgElement = document.querySelector('.carousel img');
    const leftArrow = document.querySelector('.carousel .left');
    const rightArrow = document.querySelector('.carousel .right');

    if (!imgElement || !leftArrow || !rightArrow) {
        console.error("Carousel elements not found!");
        return;
    }

    leftArrow.addEventListener('click', () => {
        currentIndex = (currentIndex === 0) ? images.length - 1 : currentIndex - 1;
        imgElement.src = images[currentIndex];
    });

    rightArrow.addEventListener('click', () => {
        currentIndex = (currentIndex === images.length - 1) ? 0 : currentIndex + 1;
        imgElement.src = images[currentIndex];
    });
}

function initializeButtons() {
    console.log("Initializing buttons...");
    const buttonScr = document.getElementById('scrollButton');
    const otziv = document.getElementById('targetElement');

    if (!buttonScr || !otziv) {
        console.error("Button elements not found!");
        return;
    }

    buttonScr.addEventListener('click', function () {
        otziv.scrollIntoView({
            behavior: 'smooth'
        });
    });

    otziv.addEventListener('click', function () {
        buttonScr.scrollIntoView({
            behavior: 'smooth'
        });
    });
}

function initializeReviews() {
    console.log("Initializing reviews...");
    const reviews = document.getElementById('reviews');

    if (!reviews) {
        console.error("Reviews element not found!");
        return;
    }

    window.addEventListener('scroll', () => {
        const reviewsPosition = reviews.getBoundingClientRect().top;
        const screenPosition = window.innerHeight / 1.3;

        if (reviewsPosition < screenPosition) {
            reviews.classList.add('visible');
        }
    });
}

function initializeTheme() {
    console.log("Initializing theme...");
    const themeToggle = document.getElementById('themeToggle');
    const body = document.body;

    if (!themeToggle || !body) {
        console.error("Theme elements not found!");
        return;
    }

    themeToggle.addEventListener('click', () => {
        body.classList.toggle('light-theme');
        themeToggle.innerHTML = body.classList.contains('light-theme') ? '<i class="fas fa-sun"></i>' : '<i class="fas fa-moon"></i>';
    });
}

function initializeComponents() {
    console.log("Initializing all components...");
    initializeCarousel();
    initializeButtons();
    initializeReviews();
    initializeTheme();
}