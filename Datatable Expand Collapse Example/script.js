
function format ( d ) {
    return ' <table class="table table-borderless">'+
            '<tr>'+
                '<td></td>'+
                '<td class="text-center no_padding">'+
                    '<h6><span class="font-weight-bold">Accessory Code: </span> '+d.accessory_code+'</h6>'+
                   
                '</td>'+
                '<td class="text-center no_padding">'+
                    '<h6><span class="font-weight-bold">Accessory Description: </span> '+d.accessory_description+'</h6>'+
                '</td>'+
            '</tr>'+
        '</table>'+
        '<div class="row">'+
            '<div class="col-12 col-xs-12 col-sm-12 col-md-3 col-lg-3 col-xl-3">'+
                '<img class="img-responsive model_image" src="https://aimagixvchekstorageaccv3.blob.core.windows.net/vchek-production-inspect-data-v3/17e3b7b7-f561-4f49-bb89-acc6d4a3c08f_23052021_110045_061192.jpg" alt="Battery Terminal">'+
                '<p class="font-weight-bold text-center">Battery Terminal</p>'+
            '</div>'+
            '<div class="col-12 col-xs-12 col-sm-12 col-md-3 col-lg-3 col-xl-3">'+
                '<img class="img-responsive model_image" src="https://aimagixvchekstorageaccv3.blob.core.windows.net/vchek-production-inspect-data-v3/17e3b7b7-f561-4f49-bb89-acc6d4a3c08f_23052021_110045_061192.jpg" alt="Battery Terminal">'+
                '<p class="font-weight-bold text-center">Battery Terminal</p>'+
            '</div>'+
            '<div class="col-12 col-xs-12 col-sm-12 col-md-3 col-lg-3 col-xl-3">'+
                '<img class="img-responsive model_image" src="https://aimagixvchekstorageaccv3.blob.core.windows.net/vchek-production-inspect-data-v3/17e3b7b7-f561-4f49-bb89-acc6d4a3c08f_23052021_110045_061192.jpg" alt="Battery Terminal">'+
                '<p class="font-weight-bold text-center">Battery Terminal</p>'+
            '</div>'+
            '<div class="col-12 col-xs-12 col-sm-12 col-md-3 col-lg-3 col-xl-3">'+
                '<div class="model_image_div">'+
                   '<img class="img-responsive model_image" src="https://aimagixvchekstorageaccv3.blob.core.windows.net/vchek-production-inspect-data-v3/17e3b7b7-f561-4f49-bb89-acc6d4a3c08f_23052021_110045_061192.jpg" alt="Battery Terminal">'+
                    '<h2 class="text">+2</h2>'+
                '</div>'+
                '<p class="font-weight-bold text-center">Battery Terminal</p>'+
            '</div>'+
        '</div>';
}

$(document).ready(function() {

    var json_obj = [
        {
            "id": "1",
            "vin_number": "VAGTWCYWBIMOW",
            "inspection_date": "23-05-2021",
            "inspection_time": "06:00 AM",
            "site_code": "CAN",
            "user_name": "Canton Site Admin",
            "activity_name": "Altima Battery",
            "activity_result": 0,
            "inspection_by": "Demo",
            "accessory_code": "GREEN",
            "accessory_description": "ALTIMA CHECK",
            "model_details":[
            {
                "model_image_url":"https://aimagixvchekstorageaccv3.blob.core.windows.net/vchek-production-inspect-data-v3/17e3b7b7-f561-4f49-bb89-acc6d4a3c08f_23052021_110045_061192.jpg",
                "model_name":"Battery Terminal",
                "model_result":0
            },
            {
                "model_image_url":"https://aimagixvchekstorageaccv3.blob.core.windows.net/vchek-production-inspect-data-v3/17e3b7b7-f561-4f49-bb89-acc6d4a3c08f_23052021_110045_061192.jpg",
                "model_name":"Battery Terminal",
                "model_result":0
            },
            {
                "model_image_url":"https://aimagixvchekstorageaccv3.blob.core.windows.net/vchek-production-inspect-data-v3/17e3b7b7-f561-4f49-bb89-acc6d4a3c08f_23052021_110045_061192.jpg",
                "model_name":"Battery Terminal",
                "model_result":0
            }
            ]
        },
        {
            "id": "2",
            "vin_number": "VAGTWCYWBIMOW",
            "inspection_date": "23-05-2021",
            "inspection_time": "06:00 AM",
            "site_code": "CAN",
            "user_name": "Canton Site Admin",
            "activity_name": "Altima Battery",
            "activity_result": 0,
            "inspection_by": "Demo",
            "accessory_code": "GREEN",
            "accessory_description": "ALTIMA CHECK",
            "model_details":[
            {
                "model_image_url":"https://aimagixvchekstorageaccv3.blob.core.windows.net/vchek-production-inspect-data-v3/17e3b7b7-f561-4f49-bb89-acc6d4a3c08f_23052021_110045_061192.jpg",
                "model_name":"Battery Terminal",
                "model_result":0
            },
            {
                "model_image_url":"https://aimagixvchekstorageaccv3.blob.core.windows.net/vchek-production-inspect-data-v3/17e3b7b7-f561-4f49-bb89-acc6d4a3c08f_23052021_110045_061192.jpg",
                "model_name":"Battery Terminal",
                "model_result":0
            },
            {
                "model_image_url":"https://aimagixvchekstorageaccv3.blob.core.windows.net/vchek-production-inspect-data-v3/17e3b7b7-f561-4f49-bb89-acc6d4a3c08f_23052021_110045_061192.jpg",
                "model_name":"Battery Terminal",
                "model_result":0
            }
            ]
        },
        {
            "id": "3",
            "vin_number": "VAGTWCYWBIMOW",
            "inspection_date": "23-05-2021",
            "inspection_time": "06:00 AM",
            "site_code": "CAN",
            "user_name": "Canton Site Admin",
            "activity_name": "Altima Battery",
            "activity_result": 0,
            "inspection_by": "Demo",
            "accessory_code": "GREEN",
            "accessory_description": "ALTIMA CHECK",
            "model_details":[
            {
                "model_image_url":"https://aimagixvchekstorageaccv3.blob.core.windows.net/vchek-production-inspect-data-v3/17e3b7b7-f561-4f49-bb89-acc6d4a3c08f_23052021_110045_061192.jpg",
                "model_name":"Battery Terminal",
                "model_result":0
            },
            {
                "model_image_url":"https://aimagixvchekstorageaccv3.blob.core.windows.net/vchek-production-inspect-data-v3/17e3b7b7-f561-4f49-bb89-acc6d4a3c08f_23052021_110045_061192.jpg",
                "model_name":"Battery Terminal",
                "model_result":0
            },
            {
                "model_image_url":"https://aimagixvchekstorageaccv3.blob.core.windows.net/vchek-production-inspect-data-v3/17e3b7b7-f561-4f49-bb89-acc6d4a3c08f_23052021_110045_061192.jpg",
                "model_name":"Battery Terminal",
                "model_result":0
            }
            ]
        },
        {
            "id": "4",
            "vin_number": "AGTWCYWBIMOWV",
            "inspection_date": "23-05-2021",
            "inspection_time": "06:00 AM",
            "site_code": "CAN",
            "user_name": "Canton Site Admin",
            "activity_name": "Altima Battery",
            "activity_result": 0,
            "inspection_by": "Demo",
            "accessory_code": "GREEN",
            "accessory_description": "ALTIMA CHECK",
            "model_details":[
            {
                "model_image_url":"https://aimagixvchekstorageaccv3.blob.core.windows.net/vchek-production-inspect-data-v3/17e3b7b7-f561-4f49-bb89-acc6d4a3c08f_23052021_110045_061192.jpg",
                "model_name":"Battery Terminal",
                "model_result":0
            },
            {
                "model_image_url":"https://aimagixvchekstorageaccv3.blob.core.windows.net/vchek-production-inspect-data-v3/17e3b7b7-f561-4f49-bb89-acc6d4a3c08f_23052021_110045_061192.jpg",
                "model_name":"Battery Terminal",
                "model_result":0
            },
            {
                "model_image_url":"https://aimagixvchekstorageaccv3.blob.core.windows.net/vchek-production-inspect-data-v3/17e3b7b7-f561-4f49-bb89-acc6d4a3c08f_23052021_110045_061192.jpg",
                "model_name":"Battery Terminal",
                "model_result":0
            }
            ]
        },
        {
            "id": "5",
            "vin_number": "VAGTWCYWBIMOW",
            "inspection_date": "23-05-2021",
            "inspection_time": "06:00 AM",
            "site_code": "CAN",
            "user_name": "Canton Site Admin",
            "activity_name": "Altima Battery",
            "activity_result": 0,
            "inspection_by": "Demo",
            "accessory_code": "GREEN",
            "accessory_description": "ALTIMA CHECK",
            "model_details":[
            {
                "model_image_url":"https://aimagixvchekstorageaccv3.blob.core.windows.net/vchek-production-inspect-data-v3/17e3b7b7-f561-4f49-bb89-acc6d4a3c08f_23052021_110045_061192.jpg",
                "model_name":"Battery Terminal",
                "model_result":0
            },
            {
                "model_image_url":"https://aimagixvchekstorageaccv3.blob.core.windows.net/vchek-production-inspect-data-v3/17e3b7b7-f561-4f49-bb89-acc6d4a3c08f_23052021_110045_061192.jpg",
                "model_name":"Battery Terminal",
                "model_result":0
            },
            {
                "model_image_url":"https://aimagixvchekstorageaccv3.blob.core.windows.net/vchek-production-inspect-data-v3/17e3b7b7-f561-4f49-bb89-acc6d4a3c08f_23052021_110045_061192.jpg",
                "model_name":"Battery Terminal",
                "model_result":0
            }
            ]
        },
        {
            "id": "6",
            "vin_number": "VAGTWCYWBIMOW",
            "inspection_date": "23-05-2021",
            "inspection_time": "06:00 AM",
            "site_code": "CAN",
            "user_name": "Canton Site Admin",
            "activity_name": "Altima Battery",
            "activity_result": 0,
            "inspection_by": "Demo",
            "accessory_code": "GREEN",
            "accessory_description": "ALTIMA CHECK",
            "model_details":[
            {
                "model_image_url":"https://aimagixvchekstorageaccv3.blob.core.windows.net/vchek-production-inspect-data-v3/17e3b7b7-f561-4f49-bb89-acc6d4a3c08f_23052021_110045_061192.jpg",
                "model_name":"Battery Terminal",
                "model_result":0
            },
            {
                "model_image_url":"https://aimagixvchekstorageaccv3.blob.core.windows.net/vchek-production-inspect-data-v3/17e3b7b7-f561-4f49-bb89-acc6d4a3c08f_23052021_110045_061192.jpg",
                "model_name":"Battery Terminal",
                "model_result":0
            },
            {
                "model_image_url":"https://aimagixvchekstorageaccv3.blob.core.windows.net/vchek-production-inspect-data-v3/17e3b7b7-f561-4f49-bb89-acc6d4a3c08f_23052021_110045_061192.jpg",
                "model_name":"Battery Terminal",
                "model_result":0
            }
            ]
        },
        {
            "id": "7",
            "vin_number": "VAGTWCYWBIMOW",
            "inspection_date": "23-05-2021",
            "inspection_time": "06:00 AM",
            "site_code": "CAN",
            "user_name": "Canton Site Admin",
            "activity_name": "Altima Battery",
            "activity_result": 0,
            "inspection_by": "Demo",
            "accessory_code": "GREEN",
            "accessory_description": "ALTIMA CHECK",
            "model_details":[
            {
                "model_image_url":"https://aimagixvchekstorageaccv3.blob.core.windows.net/vchek-production-inspect-data-v3/17e3b7b7-f561-4f49-bb89-acc6d4a3c08f_23052021_110045_061192.jpg",
                "model_name":"Battery Terminal",
                "model_result":0
            },
            {
                "model_image_url":"https://aimagixvchekstorageaccv3.blob.core.windows.net/vchek-production-inspect-data-v3/17e3b7b7-f561-4f49-bb89-acc6d4a3c08f_23052021_110045_061192.jpg",
                "model_name":"Battery Terminal",
                "model_result":0
            },
            {
                "model_image_url":"https://aimagixvchekstorageaccv3.blob.core.windows.net/vchek-production-inspect-data-v3/17e3b7b7-f561-4f49-bb89-acc6d4a3c08f_23052021_110045_061192.jpg",
                "model_name":"Battery Terminal",
                "model_result":0
            }
            ]
        },
        {
            "id": "8",
            "vin_number": "VAGTWCYWBIMOW",
            "inspection_date": "23-05-2021",
            "inspection_time": "06:00 AM",
            "site_code": "CAN",
            "user_name": "Canton Site Admin",
            "activity_name": "Altima Battery",
            "activity_result": 0,
            "inspection_by": "Demo",
            "accessory_code": "GREEN",
            "accessory_description": "ALTIMA CHECK",
            "model_details":[
            {
                "model_image_url":"https://aimagixvchekstorageaccv3.blob.core.windows.net/vchek-production-inspect-data-v3/17e3b7b7-f561-4f49-bb89-acc6d4a3c08f_23052021_110045_061192.jpg",
                "model_name":"Battery Terminal",
                "model_result":0
            },
            {
                "model_image_url":"https://aimagixvchekstorageaccv3.blob.core.windows.net/vchek-production-inspect-data-v3/17e3b7b7-f561-4f49-bb89-acc6d4a3c08f_23052021_110045_061192.jpg",
                "model_name":"Battery Terminal",
                "model_result":0
            },
            {
                "model_image_url":"https://aimagixvchekstorageaccv3.blob.core.windows.net/vchek-production-inspect-data-v3/17e3b7b7-f561-4f49-bb89-acc6d4a3c08f_23052021_110045_061192.jpg",
                "model_name":"Battery Terminal",
                "model_result":0
            }
          ]
        }
    ];

    var table = $('#example').DataTable({
        "data" : json_obj,
        'columns': [
            {
                'className':   'details-control',
                'orderable':      false,
                'data':           null,
                'defaultContent': ''
            },
            { 'data': 'vin_number' },
            { 'data': 'inspection_by' },
            { 'data': 'user_name' },
            { 'data': 'activity_name' },
            { 'data': 'activity_result' },
            { 'data': 'inspection_by' }
        ],
        "bPaginate": false,
        "aaSorting": [],
        "info": false,
    });

    // Add event listener for opening and closing details
    $('#example tbody').on('click', 'td.details-control', function(){
        var tr = $(this).closest('tr');
        var row = table.row( tr );

        if(row.child.isShown()){
            // This row is already open - close it
            row.child.hide();
            tr.removeClass('shown');
        } else {
            // Open this row
            row.child(format(row.data())).show();
            tr.addClass('shown');
        }
    });

    // Handle click on "Expand All" button
    $('#btn-show-all-children').on('click', function(){
        // Enumerate all rows
        table.rows().every(function(){
            // If row has details collapsed
            if(!this.child.isShown()){
                // Open this row
                this.child(format(this.data())).show();
                $(this.node()).addClass('shown');
            }
        });
    });

    // Handle click on "Collapse All" button
    $('#btn-hide-all-children').on('click', function(){
        // Enumerate all rows
        table.rows().every(function(){
            // If row has details expanded
            if(this.child.isShown()){
                // Collapse row details
                this.child.hide();
                $(this.node()).removeClass('shown');
            }
        });
    });
});



