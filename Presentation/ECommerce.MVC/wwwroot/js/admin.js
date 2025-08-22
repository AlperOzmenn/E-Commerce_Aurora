document.addEventListener("DOMContentLoaded", function () {
    const profileBtn = document.getElementById("profileImg");
    const dropdown = document.getElementById("profileDropdown");

    if (profileBtn && dropdown) {
        profileBtn.addEventListener("click", function (e) {
            dropdown.style.display = dropdown.style.display === "block" ? "none" : "block";
            e.stopPropagation();
        });

        document.addEventListener("click", function (e) {
            if (!profileBtn.contains(e.target) && !dropdown.contains(e.target)) {
                dropdown.style.display = "none";
            } 
        });
    } 
});
