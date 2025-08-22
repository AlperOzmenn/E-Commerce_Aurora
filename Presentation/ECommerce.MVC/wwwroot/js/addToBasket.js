
    //$(document).ready(function () {
    //    $("form.add-to-basket-form").submit(function (e) {
    //        e.preventDefault();

    //        var form = $(this);
    //        var formData = form.serialize();

    //        $.ajax({
    //            type: "POST",
    //            url: form.attr("action"),
    //            data: formData,
    //            success: function (response) {
    //                if (response.success) {
    //                    // Başarılıysa kullanıcıya bilgi ver
    //                    alert("Ürün sepete eklendi.");
    //                }
    //            },
    //            error: function () {
    //                alert("Sepete eklenirken bir hata oluştu.");
    //            }
    //        });
    //    });
//    });

$(document).ready(function () {
    $("form.add-to-basket-form").submit(function (e) {
        e.preventDefault();

        var form = $(this);
        var formData = form.serialize();

        $.ajax({
            type: "POST",
            url: form.attr("action"),
            data: formData,
            success: function (response) {
                if (response.success) {
                    alert(response.message); // ✅ Kullanıcıya bilgi ver
                } else {
                    alert("Sepete eklenemedi.");
                }
            },
            error: function () {
                alert("Bir hata oluştu. Lütfen tekrar deneyin.");
            }
        });
    });
});