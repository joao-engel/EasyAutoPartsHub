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

//Mascaras de campos de formulários
function fn_mascaras() {
    // Mascara para telefone fixo e celulares
    var maskTelefones = function (val) {
        return val.replace(/\D/g, '').length === 11 ? '(00) 00000-0000' : '(00) 0000-00009';
    },
        spTelefonesOptions = {
            onKeyPress: function (val, e, field, options) {
                field.mask(maskTelefones.apply({}, arguments), options);
            }
        };
    $('[data-telefones]').mask(maskTelefones, spTelefonesOptions);

    $('.moeda').mask("#.##0,00", { reverse: true });
    $('.cep').mask("00000-000", { reverse: false });
    $('.data').mask("00/00/0000", { reverse: false });
    $('.datetimepicker').mask('00/00/0000 00:00', { reverse: false });
    $('.mesRef').mask('00/0000', { reverse: false });
    $('.cnpj').mask("00.000.000/0000-00", { reverse: false });
    $('.cpf').mask("000.000.000-00", { reverse: false });
    $('.number').mask('#.##0', { reverse: true });
}

$(document).on('focusout', 'input[type="text"], textarea', function () {
    this.value = this.value.trim();
});

$(function () {
    fn_mascaras();
});

function abrirModalClientes() {
    const modal = new bootstrap.Modal('#modal-clientes');
    modal.show();
}

function filtro(idTabela, input) {
    const termo = input.value.toLowerCase();
    const linhas = document.querySelectorAll(`#${idTabela} tbody tr`);

    linhas.forEach(linha => {
        const textoLinha = linha.innerText.toLowerCase();
        const corresponde = textoLinha.includes(termo);
        linha.style.display = corresponde ? '' : 'none';
    });
}