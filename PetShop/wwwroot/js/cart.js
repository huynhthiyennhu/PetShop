$(document).ready(function () {
    $('.add-to-cart').click(function (e) {
        e.preventDefault();

        var petId = $(this).data('id');

        $.ajax({
            url: '/Cart/AddToCart',
            type: 'POST',
            data: { id: petId },
            dataType: 'json',
            success: function (response) {
                if (response.success) {
                    // Hiển thị thông báo thành công
                    showAlert(response.message);
                    $('.badge').text(response.totalQuantity); // Cập nhật số lượng giỏ hàng
                    updateCartList(response.items); // Cập nhật danh sách mặt hàng
                } else {
                    // Hiển thị thông báo lỗi
                    showAlert(response.message);
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.error('AJAX Error:', textStatus, errorThrown);
                showAlert('Có lỗi xảy ra. Vui lòng thử lại.');
            }
        });
    });
});

function updateCartList(items) {
    var cartList = $('.cart-list');
    cartList.empty(); // Xóa danh sách hiện tại

    $.each(items, function (index, item) {
        var price = item.Price.toLocaleString('vi-VN', { style: 'currency', currency: 'VND' });
        cartList.append(`
            <li>
                <a href="#" class="photo"><img src="https://localhost:7254/images/pet/${item.ImageUrl}" class="cart-thumb" alt="" /></a>
                <h6><a asp-action="Detail" asp-controller="Pets" asp-route-id="${item.PetId}">${item.Name}</a></h6>
                <p>1x - <span class="price">${price}</span></p>
            </li>
        `);
    });
}

// Hàm hiển thị thông báo (có thể sử dụng thư viện khác để cải thiện)
function showAlert(message) {
    alert(message); // Thay thế bằng thông báo đẹp hơn nếu cần
}
