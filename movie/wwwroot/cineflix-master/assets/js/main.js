'use strict';

// variables for navbar menu toggle
//const header = document.querySelector('header');
//const nav = document.querySelector('nav');
//const navbarMenuBtn = document.querySelector('.navbar-menu-btn');

//// variables for navbar search toggle
//const navbarForm = document.querySelector('.navbar-form');
//const navbarFormCloseBtn = document.querySelector('.navbar-form-close');
//const navbarSearchBtn = document.querySelector('.navbar-search-btn');

//// navbar menu toggle function
//function navIsActive() {
//    header.classList.toggle('active');
//    nav.classList.toggle('active');
//    navbarMenuBtn.classList.toggle('active');
//}

//navbarMenuBtn.addEventListener('click', navIsActive);

//// navbar search toggle function
//const searchBarIsActive = () => {
//    navbarForm.classList.toggle('active')

//};

//navbarSearchBtn.addEventListener('click', searchBarIsActive);
//navbarFormCloseBtn.addEventListener('click', searchBarIsActive);

//document.addEventListener("DOMContentLoaded", function () {
//    const form = document.querySelector(".navbar-form");
//    const searchInput = document.querySelector(".navbar-form-search");

//    form.addEventListener("submit", function (event) {
//        event.preventDefault(); 
//        const searchValue = search.value; 
//        console.log(searchValue); 
//    });
//});

//document.addEventListener("DOMContentLoaded", function () {
//    var form = document.querySelector('.navbar-form');
//    var input = form.querySelector('.navbar-form-search');

//    form.addEventListener('submit', function (e) {
//        e.preventDefault();
//        var searchValue = input.value.trim();
//        if (searchValue !== '') {
//            var searchInput = document.createElement('input');
//            searchInput.type = 'hidden';
//            searchInput.name = 'search';
//            searchInput.value = searchValue;
//            form.appendChild(searchInput);
//            form.submit();
//        }
//    });

//    var closeButton = form.querySelector('.navbar-form-close');
//    closeButton.addEventListener('click', function () {
//        input.value = '';
//    });
//});




//slide-bar
let slideIndex = 0;
const slides = document.querySelectorAll('.banner-card');
const dots = document.querySelectorAll('.dot');

function showSlides(n) {
    if (n >= slides.length) slideIndex = 0;
    if (n < 0) slideIndex = slides.length - 1;

    for (let i = 0; i < dots.length; i++) {
        dots[i].className = dots[i].className.replace(' active', '');
    }

    dots[slideIndex].className += ' active';
}

function currentSlide(n) {
    slideIndex = n;
    updateSliderPosition();
    showSlides(slideIndex);
}

function nextSlide() {
    slideIndex++;
    if (slideIndex >= slides.length) slideIndex = 0;
    updateSliderPosition();
    showSlides(slideIndex);
}

function prevSlide() {
    slideIndex--;
    if (slideIndex < 0) slideIndex = slides.length - 1;
    updateSliderPosition();
    showSlides(slideIndex);
}

function updateSliderPosition() {
    const bannerContainer = document.querySelector('.banner-container');
    bannerContainer.style.transform = `translateX(-${slideIndex * 100}%)`;
}

setInterval(nextSlide, 5000); // Change image every 5 seconds

document.addEventListener('DOMContentLoaded', () => {
    showSlides(slideIndex);
});

