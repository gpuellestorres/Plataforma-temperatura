$(function() {

    Morris.Area({
        element: 'morris-area-chart',
        data: [{
            period:"2015-03-01",
            Molino_1: 2666,
            Molino_2: 2000,
            Molino_3: 2647
        }, {
            period: "2015-03-02",
            Molino_1: 2778,
            Molino_2: 2294,
            Molino_3: 2441
        }, {
            period: "2015-03-03",
            Molino_1: 4912,
            Molino_2: 1969,
            Molino_3: 2501
        }, {
            period: "2015-03-04",
            Molino_1: 3767,
            Molino_2: 3597,
            Molino_3: 5689
        }, {
            period: "2015-03-05",
            Molino_1: 6810,
            Molino_2: 1914,
            Molino_3: 2293
        }, {
            period: "2015-03-06",
            Molino_1: 5670,
            Molino_2: 4293,
            Molino_3: 1881
        }, {
            period: "2015-03-07",
            Molino_1: 4820,
            Molino_2: 3795,
            Molino_3: 1588
        }, {
            period: "2015-03-08",
            Molino_1: 15073,
            Molino_2: 5967,
            Molino_3: 5175
        }, {
            period: "2015-03-09",
            Molino_1: 10687,
            Molino_2: 4460,
            Molino_3: 2028
        }, {
            period: "2015-03-10",
            Molino_1: 8432,
            Molino_2: 5713,
            Molino_3: 1791
        }],
        xkey: 'period',
        ykeys: ['Molino_1', 'Molino_2', 'Molino_3'],
        labels: ['Molino 1', 'Molino 2', 'Molino 3'],
        pointSize: 2,
        hideHover: 'auto',
        resize: true
    });

    Morris.Donut({
        element: 'morris-donut-chart',
        data: [{
            label: "Download Sales",
            value: 12
        }, {
            label: "In-Store Sales",
            value: 30
        }, {
            label: "Mail-Order Sales",
            value: 20
        }],
        resize: true
    });

    Morris.Bar({
        element: 'morris-bar-chart',
        data: [{
            y: '2006',
            a: 100,
            b: 90
        }, {
            y: '2007',
            a: 75,
            b: 65
        }, {
            y: '2008',
            a: 50,
            b: 40
        }, {
            y: '2009',
            a: 75,
            b: 65
        }, {
            y: '2010',
            a: 50,
            b: 40
        }, {
            y: '2011',
            a: 75,
            b: 65
        }, {
            y: '2012',
            a: 100,
            b: 90
        }],
        xkey: 'y',
        ykeys: ['a', 'b'],
        labels: ['Series A', 'Series B'],
        hideHover: 'auto',
        resize: true
    });

});
