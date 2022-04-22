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

                var no_data_div_html = '<div id="no_data_div">\
                                    <div class="row">\
                                        <div class="col-12 col-sm-12 col-xs-12 col-md-12 col-lg-12 col-xl-12 text-center">\
                                            <h4>No data available</h4>\
                                            <br />\
                                        </div>\
                                    </div>\
                                </div>';
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
                                var current_resource_div_html = '<div id="current_resource_main_div">';
                                current_resource_div_html += '<tr id="current_resource_div' + response.Data[i].customer_registration_checker_id + '"><td>' + response.Data[i].CRN + '</td> <td>' + response.Data[i].created_on + '</td><td  style="padding-bottom: 0px;padding-top: 5px;" <button type="button" class="btn btn-primary move_current_resource_data" id="move_btn" onclick="move_current_resource_data(' + response.Data[i].customer_registration_checker_id + ')">Move</button></td> </tr>'
                                content_div_html += '</div>';
                                $('#current_resource_datatable tbody').current_resource_div_html;
                              
                            }
                            else if (sample_response.Data[i].trained_resource_group_id == "002") {
                                //002 Record Table Append
                                var new_resource_div_html = '<div id="new_resource_main_div">'
                                new_resource_div_html += '<tr><td>' + response.Data[i].CRN + '</td> <td>' + response.Data[i].created_on + '</td></tr>';
                                new_resource_div_html += '</div>';
                                $('#new_resource_datatable tbody').new_resource_div_html;
                            }
                            else {
                                $('#current_resource_datatable').html(no_data_div_html);
                                $('#new_resource_datatable').html(no_data_div_html);
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
                    window.location.href = '/Home/DataMigration/';
                }
            }
            catch (err) {
                console.log(err);
                $('#current_resource_datatable').html(no_data_div_html);
                $('#new_resource_datatable').html(no_data_div_html);
            }
        },
        error: function (response) {
            $('#current_resource_datatable').html(no_data_div_html);
            $('#new_resource_datatable').html(no_data_div_html);
           }
    });

}


function move_current_resource_data(customer_registration_checker_id) {
    $("#success_modal").modal("show");
    $("#move_btn").attr("customer_registration_checker_id", customer_registration_checker_id);
}

$("#move_btn").click(function (e) {
    e.preventDefault();
    var obj = {};
    obj.customer_registration_checker_id = $("#move_btn").attr("customer_registration_checker_id");

    var dataObject = JSON.stringify(obj);
      $('#move_btn').attr("disabled", true);
    //$('#cancel_btn').attr("disabled", true);

    $.ajax({
        type: "POST",
        url: "/Home/data_migration_on_start",
        data: { dataObject: dataObject },

        success: function (response) {
            console.log(response);

            if (response.StatusCode == "200") { //Success
                $('#current_resource_div' + JSON.parse(dataObject).customer_registration_checker_id).remove();
                $('#new_resource_div' + JSON.parse(dataObject).customer_registration_checker_id).add();
                $("#success_modal").modal("hide");

                var get_current_resource_main_div = $('#current_resource_main_div').html();
                if (get_current_resource_main_div == undefined || get_current_resource_main_div == "") {

                    var no_data_div_html = '<div id="no_data_div">\
                                    <div class="row">\
                                        <div class="col-12 col-sm-12 col-xs-12 col-md-12 col-lg-12 col-xl-12 text-center">\
                                            <h4>No data available</h4>\
                                            <br />\
                                        </div>\
                                    </div>\
                                </div>';

                    $('#current_resource_datatable').html(no_data_div_html);
                    
                }
            }
            else {
                $("#failure_modal").modal("show");
            }

        },
        error: function (response) {
            $("#failure_modal").modal("show");
        }
    });
});

