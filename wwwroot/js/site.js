// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// Get a reference to the loading indicator element
const loadingIndicator = document.getElementById('loading-indicator');
if (loadingIndicator) {
    // Hide it once the page loads
    window.addEventListener('load', () => {
        loadingIndicator.style.display = 'none';
    });
}

function showLoading() {
    loadingIndicator.style.display = 'flex';
}

function hideLoading() {
    loadingIndicator.style.display = 'none';
}


