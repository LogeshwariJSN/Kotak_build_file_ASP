$(document).ready(function () {
    get_data_migration_data();
});

function get_data_migration_data() {
    var obj = {}
    var send_data = JSON.stringify(obj);
    $('#data_div').hide();
    $('#no_data_div').hide();
    $('#loading_div').show();
    $.ajax({
        type: "POST",
        data: send_data,
        url: "/Home/get_data_migration_data/",
        contentType: "application/json",
        datatype: "json",
        success: function (response) {
            console.log(response);
            try {
                //var response = {
                //    "StatusCode": 200,
                //    "Data": [
                //        {
                //            "customer_registration_checker_id": 1,
                //            "CRN": "12345",
                //            "trained_resource_group_id": "001",
                //            "created_on": "06-04-2022 01:02:03"
                //        },
                //        {
                //            "customer_registration_checker_id": 1,
                //            "CRN": "12345",
                //            "trained_resource_group_id": "001",
                //            "created_on": "06-04-2022 01:02:03"
                //        },
                //        {
                //            "customer_registration_checker_id": 1,
                //            "CRN": "12345",
                //            "trained_resource_group_id": "002",
                //            "created_on": "06-04-2022 01:02:03"
                //        },
                //        {
                //            "customer_registration_checker_id": 1,
                //            "CRN": "12345",
                //            "trained_resource_group_id": "002",
                //            "created_on": "06-04-2022 01:02:03"
                //        },
                //        {
                //            "customer_registration_checker_id": 1,
                //            "CRN": "12345",
                //            "trained_resource_group_id": "002",
                //            "created_on": "06-04-2022 01:02:03"
                //        }
                //    ]
                //};
                if (response.StatusCode == 200) {
                    if (response.Data.length == 0) {
                        $('#data_div').hide();
                        $('#loading_div').hide();
                        $('#no_data_div').show();
                    } else {
                        $('#current_resource_table tbody').empty();
                        $('#new_resource_table tbody').empty();
                        var current_resource_tr_html = "",
                            new_resource_tr_html = "";
                        for (var i = 0; i < response.Data.length; i++) {
                            if (response.Data[i].trained_resource_group_id == "001") {
                                //001 Record Table Append
                                current_resource_tr_html += '<tr id="current_resource_tr' + response.Data[i].customer_registration_checker_id + '"><td id="crn_td' + response.Data[i].customer_registration_checker_id + '">' + response.Data[i].CRN + '</td> <td id="registered_at_td' + response.Data[i].customer_registration_checker_id + '">' + response.Data[i].created_on + '</td><td><button type="button" class="btn btn-primary move_btn" id="move_btn' + response.Data[i].customer_registration_checker_id + '" onclick="move_current_resource_data(' + response.Data[i].customer_registration_checker_id + ')">Move</button></td></tr>';
                            } else if (response.Data[i].trained_resource_group_id == "002") {
                                //002 Record Table Append
                                new_resource_tr_html += '<tr><td>' + response.Data[i].CRN + '</td> <td>' + response.Data[i].created_on + '</td></tr>';
                            }
                        }
                        $('#current_resource_table tbody').append(current_resource_tr_html);
                        $('#new_resource_table tbody').append(new_resource_tr_html);
                        $('#data_div').show();
                        $('#loading_div').hide();
                        $('#no_data_div').hide();
                    }
                } else {
                    $('#data_div').hide();
                    $('#loading_div').hide();
                    $('#no_data_div').show();
                    popup("Something went wrong, Please try again");
                }
            } catch (err) {
                console.log(err);
                $('#data_div').hide();
                $('#loading_div').hide();
                $('#no_data_div').show();
                popup("Something went wrong, Please try again");
            }
        },
        error: function (response) {
            $('#data_div').hide();
            $('#no_data_div').show();
            popup("Something went wrong, Please try again");
        }
    });
}

function move_current_resource_data(customer_registration_checker_id) {
    
    
    $('#move_btn' + customer_registration_checker_id).html("<i class='fa fa-spinner fa-spin '></i> Loading..");
    $('#move_btn' + customer_registration_checker_id).button('loading');
    $('.move_btn').attr("disabled", true);
    var JsonData = JSON.stringify({ customer_registration_checker_id: customer_registration_checker_id });
    $.ajax({
        type: "POST",
        data: JsonData,
        url: "/Home/data_migration_on_start/",
        contentType: "application/json",
        datatype: "json",
        success: function (response) {
            console.log(response);
            try {
                //var response = {
                //    "StatusCode": 200
                //};
                if (response.StatusCode == 200) {
                    $('#move_btn' + JSON.parse(JsonData).customer_registration_checker_id).html("Move");
                $('.move_btn').attr("disabled", false);

                var get_crn = $('#crn_td' + JSON.parse(JsonData).customer_registration_checker_id).html();
                var get_registered_at = $('#registered_at_td' + JSON.parse(JsonData).customer_registration_checker_id).html();
                $('#new_resource_table tbody').append("<tr><td>" + get_crn + "</td><td>" + get_registered_at + "</td></tr>");

                    $('#current_resource_tr' + JSON.parse(JsonData).customer_registration_checker_id).remove();
                    popup("Data moved successfully");
                } else {
                    $('#move_btn' + JSON.parse(JsonData).customer_registration_checker_id).html("Move");
                $('.move_btn').attr("disabled", false);
                popup("Something went wrong, Please try again");
                }
            } catch (err) {
                console.log(err);
                $('#move_btn' + JSON.parse(JsonData).customer_registration_checker_id).html("Move");
                $('.move_btn').attr("disabled", false);
                popup("Something went wrong, Please try again");
            }
        },
        error: function (response) {
            $('#move_btn' + JSON.parse(JsonData).customer_registration_checker_id).html("Move");
            $('.move_btn').attr("disabled", false);
            popup("Something went wrong, Please try again");
        }
    });
}

function popup(Message) {
    var html = '<div class="modal fade" id="alert_popup" role="dialog" data-keyboard="false" data-backdrop="static"><div class="modal-dialog"><div class="modal-content"><div class="modal-body"><h4 class="text-center">' + Message + '</h4><br><center><button id="modalButton1" type="button" class="btn btn-primary">Okay</button></center></div></div>';
    document.getElementById("alert_popup_div").innerHTML = html;
    $('#alert_popup').modal('show');
    $("#modalButton1").click(function () {
        $('#alert_popup').modal('hide');
    });
}