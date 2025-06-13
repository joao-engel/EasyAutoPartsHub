const appGlobal = {
    formatterValueIndex: function (value, index) {
        return value.toLocaleString('pt-br', {
            minimumFractionDigits: 0,
            maximumFractionDigits: 0
        });
    },
    formatterValor: function (valor) {
        return valor.value.toLocaleString('pt-br', {
            minimumFractionDigits: 0,
            maximumFractionDigits: 0
        });
    },
    color: 'black',
    fontWeight: 'normal',
    textBorderColor: 'black',
    textBorderWidth: 0,
    // splitLine linhas no meio do grafico
    // https://echarts.apache.org/en/option.html#yAxis.splitLine.lineStyle.color
    splitLine: {
        lineStyle: {
            color: "#000"
        }
    },
    // axisLine texto abaixo do grafico [Seg, Ter, ....]
    // https://echarts.apache.org/en/option.html#xAxis.axisLine.lineStyle.color
    axisLine: {
        lineStyle: {
            color: "#000"
        }
    }
};