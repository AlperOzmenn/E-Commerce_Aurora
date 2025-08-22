function toggleProfileDropdown() {
    const dropdown = document.getElementById("profileDropdown");
    dropdown.classList.toggle("active");
}

document.addEventListener("click", function (event) {
    const dropdown = document.getElementById("profileDropdown");
    const profilePic = document.querySelector(".profile-pic");

    if (!dropdown.contains(event.target) && !profilePic.contains(event.target)) {
        dropdown.classList.remove("active");
    }
});