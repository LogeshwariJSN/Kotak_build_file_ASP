function get_data_migration_data() {
    //var main_obj = { trained_resource_group_id: trained_resource_group_id}
    var send_data = JSON.stringify();

    console.log(send_data);
    $.ajax({
        type: "POST",
        data: send_data,
        url: "/Home/get_data_migration_data/",
        contentType: "application/json",
        datatype: "json",
        success: function (response) {
            console.log(response);
            try {
                if (response.StatusCode == 200) {
                    if (response.Data.length == 0) {
                        $('#current_resource_datatable').hide();
                        $('#new_resource_datatable').hide();
                        $('#no_data_div').show();
                        $('#current_resource_datatable tbody').empty();
                        $('#new_resource_datatable tbody').empty();
                    }
                    else {
                        $('#current_resource_datatable tbody').empty();
                        $('#new_resource_datatable tbody').empty();
                       

                        for (var i = 0; i < response.Data.length; i++) {
                            if (response.Data[i].trained_resource_group_id == "001") {
                                //001 Record Table Append
                                $('#current_resource_datatable tbody').append('<tr><td>' + response.Data.DataMigrationData[i].CRN + '</td> <td>' + response.Data[i].created_on + '</td></tr>');
                            }
                            else if (sample_response.Data[i].trained_resource_group_id == "002") {
                                //002 Record Table Append
                                $('#new_resource_datatable tbody').append('<tr><td>' + response.Data.DataMigrationData[i].CRN + '</td> <td>' + response.Data.DataMigrationData[i].created_on + '</td></tr>');
                            }
                        }

                        $('#current_resource_datatable').show();
                        $('#new_resource_datatable').show
                        $('#no_data_div').hide();
                    }
                }
                else {
                    $('#current_resource_datatable').hide();
                    $('#new_resource_datatable').hide();
                    $('#no_data_div').show();
                   
                }
            }
            catch (err) {
                console.log(err);
                $('#current_resource_datatable').hide();
                $('#new_resource_datatable').hide();
                $('#no_data_div').show();
            }
        },
        error: function (response) {
            $('#current_resource_datatable').hide();
            $('#new_resource_datatable').hide();
            $('#no_data_div').show();
        }
    });

}
