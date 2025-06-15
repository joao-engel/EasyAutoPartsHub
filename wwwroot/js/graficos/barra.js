function fn_graf_barra(id, labels, values_fat, values_meta) {
    const chartDom = document.getElementById(id);
    const myChart = echarts.init(chartDom);

    const option = {
        grid: {
            left: '0%',
            right: '0%',
            top: '20%',
            bottom: '2%',
            containLabel: true
        },
        tooltip: {
            trigger: 'axis',
            axisPointer: { type: 'shadow' }
        },
        legend: {
            data: ['Faturamento', 'Meta']
        },
        xAxis: {
            type: 'category',
            data: labels,
            axisTick: { alignWithLabel: true }
        },
        yAxis: {
            type: 'value',
            axisLabel: { show: false }, 
            splitLine: { show: false }, 
            axisLine: { show: false },  
            axisTick: { show: false }   
        },
        series: [
            {
                name: 'Faturamento',
                type: 'bar',
                barMaxWidth: 50,
                itemStyle: {
                    color: '#4e73df' // Cor das barras
                },
                label: {
                    show: true,
                    position: 'inside',
                    formatter: function (params) {
                        return params.value.toFixed(0);
                    },
                    color: '#fff',
                    fontSize: 12,
                    fontWeight: 'bold'
                },
                data: values_fat
            },
            {
                name: 'Meta',
                type: 'line',
                symbol: 'circle',
                symbolSize: 8,
                lineStyle: {
                    color: '#e74a3b', // Cor da linha da meta
                    width: 3
                },
                itemStyle: {
                    color: '#e74a3b'
                },
                data: values_meta
            }
        ]
    };

    myChart.setOption(option);
}