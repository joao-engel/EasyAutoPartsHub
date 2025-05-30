//Tela de carregamento do AJAX
$(document).ajaxStart(function () {
    $('#ajax-loader').removeClass('d-none');
});
$(document).ajaxStop(function () {
    $('#ajax-loader').addClass('d-none');
});

//Mensagem de erro do AJAX
$(document).on("ajaxError", function (event, request, settings) {
    Swal.fire({
        icon: 'error',
        title: 'Oops...',
        html: request.responseJSON.detail,
        showClass: {
            popup: 'animate__animated animate__fadeInDown'
        },
        hideClass: {
            popup: 'animate__animated animate__fadeOutUp'
        }
    });
});

//submit de formulários com AJAX e exibição de mensagens de sucesso
function fn_submit(evt, formID, urlSubmit, urlRetorno, reloadPage) {

    evt.preventDefault();

    $.ajax({
        method: "POST",
        url: urlSubmit,
        data: $(formID).serializeArray()
    }).done(function (data, textStatus, jqXHR) {

        Swal.fire({
            title: data,
            text: "",
            icon: 'success',
            showCancelButton: false,
            confirmButtonColor: '#3085d6',
            confirmButtonText: 'Ok',
            showClass: {
                popup: 'animate__animated animate__fadeInDown'
            },
            hideClass: {
                popup: 'animate__animated animate__fadeOutUp'
            }
        }).then((result) => {
            if (result.value) {
                location.replace(urlRetorno);
            } else {
                if (reloadPage == true) {
                    window.location.reload();
                }
            }
        });
    });
}