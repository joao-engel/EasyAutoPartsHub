function fn_graf_pizza(id, labels, values) {
    const chartDom = document.getElementById(id);
    const myChart = echarts.init(chartDom);

    const data = labels.map((label, index) => ({
        value: values[index],
        name: label
    }));

    const option = {
        legend: { top: '5%', left: 'center' },
        series: [{
            top: '5%',
            name: 'Pedidos',
            type: 'pie',
            radius: ['40%', '70%'],
            itemStyle: {
                borderRadius: 10, borderColor: '#fff', borderWidth: 2
            },
            label: {
                show: true,
                position: 'inside',
                formatter: function (params) {
                    return params.value;
                },
                color: '#fff',
                fontSize: 12,
                fontWeight: 'bold'
            },
            labelLine: { show: false },
            data: data
        }]
    };

    myChart.setOption(option);
}