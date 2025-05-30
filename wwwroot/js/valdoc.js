// onblur="validaDoc(this)"
function validaDoc(campo) {
    let docSemFormt = campo.value.replace(/[^\d]+/g, '');

    if (docSemFormt != '') {
        if (docSemFormt.length == 11) {
            if (!validarCPF(docSemFormt)) {
                campo.value = "";

                Swal.fire({
                    title: 'CPF inválido',
                    html: '',
                    icon: 'error',
                    confirmButtonColor: '#3085d6',
                    confirmButtonText: 'OK',
                    showClass: {
                        popup: 'animate__animated animate__fadeInDown'
                    },
                    hideClass: {
                        popup: 'animate__animated animate__fadeOutUp'
                    }
                }).then((result) => {
                    campo.focus();
                });
            }
        } else if (docSemFormt.length == 14) {
            if (!validarCNPJ(docSemFormt)) {
                campo.value = "";

                Swal.fire({
                    title: 'CNPJ inválido',
                    html: '',
                    icon: 'error',
                    confirmButtonColor: '#3085d6',
                    confirmButtonText: 'OK',
                    showClass: {
                        popup: 'animate__animated animate__fadeInDown'
                    },
                    hideClass: {
                        popup: 'animate__animated animate__fadeOutUp'
                    }
                }).then((result) => {
                    campo.focus();
                });
            }
        }
    }

    // functions private
    function validarCPF(cpf) {

        if (cpf.length != 11 ||
            cpf == "00000000000" || cpf == "11111111111" || cpf == "22222222222" ||
            cpf == "33333333333" || cpf == "44444444444" || cpf == "55555555555" ||
            cpf == "66666666666" || cpf == "77777777777" || cpf == "88888888888" ||
            cpf == "99999999999") {
            return false;
        }

        var soma = 0;
        for (var i = 0; i < 9; i++)
            soma += parseInt(cpf.charAt(i)) * (10 - i);
        var resto = 11 - (soma % 11);
        if (resto == 10 || resto == 11)
            resto = 0;
        if (resto != parseInt(cpf.charAt(9))) {
            return false;
        }
        soma = 0;
        for (i = 0; i < 10; i++)
            soma += parseInt(cpf.charAt(i)) * (11 - i);
        resto = 11 - (soma % 11);
        if (resto == 10 || resto == 11)
            resto = 0;
        if (resto != parseInt(cpf.charAt(10))) {
            return false;
        }
        return true;
    }
    // functions private
    function validarCNPJ(cnpj) {
        if (cnpj == "00000000000000" || cnpj == "11111111111111" ||
            cnpj == "22222222222222" || cnpj == "33333333333333" ||
            cnpj == "55555555555555" || cnpj == "44444444444444" ||
            cnpj == "66666666666666" || cnpj == "77777777777777" ||
            cnpj == "88888888888888" || cnpj == "99999999999999") {
            return false;
        }
        var tamanho = cnpj.length - 2;
        var numeros = cnpj.substring(0, tamanho);
        var digitos = cnpj.substring(tamanho);
        var soma = 0;
        var pos = tamanho - 7;
        for (var i = tamanho; i >= 1; i--) {
            soma += numeros.charAt(tamanho - i) * pos--;
            if (pos < 2)
                pos = 9;
        }
        var resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
        if (resultado != digitos.charAt(0)) {
            return false;
        }
        tamanho = tamanho + 1;
        numeros = cnpj.substring(0, tamanho);
        soma = 0;
        pos = tamanho - 7;
        for (i = tamanho; i >= 1; i--) {
            soma += numeros.charAt(tamanho - i) * pos--;
            if (pos < 2)
                pos = 9;
        }
        resultado = soma % 11 < 2 ? 0 : 11 - soma % 11;
        if (resultado != digitos.charAt(1)) {
            return false;
        }
        return true;
    }
}
