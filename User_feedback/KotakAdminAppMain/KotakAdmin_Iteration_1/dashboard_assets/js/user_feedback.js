$(document).ready(function () {

    //filter User Answer data fetch
   
    //Rating range slider
    $('.noUi-handle').on('click', function () {
        $(this).width(50);
    });
    var rangeSlider = document.getElementById('slider-range');
    var moneyFormat = wNumb({
        decimals: 0
    });
    noUiSlider.create(rangeSlider, {
        start: [1, 3],
        step: 1,
        range: {
            'min': [1],
            'max': [5]
        },
        format: moneyFormat,
        connect: true
    });

    // Set visual min and max values and also update value hidden form inputs
    rangeSlider.noUiSlider.on('update', function (values, handle) {
        document.getElementById('slider-range-value1').innerHTML = values[0];
        document.getElementById('slider-range-value2').innerHTML = values[1];
        $('#min-value').val(values[0]);
        $('#max-value').val(values[1]);
    });

    
});


$('#custom_popup').hide();
$('#answer1_popup').hide();
$('#answer2_popup').hide();


$(".question1").select2({
    tags: true,
    placeholder: " Select options",
    multiple: true,
})
$(".question2").select2({
    tags: true,
    placeholder: " Select options",
    multiple: true,
})

//Filters Custom popup
$("#myPopover").popover({
    html: true,
    placement: "bottom",
    content: function () {
        return $(".popovercontent_div").html();
    }
});

var is_filter_open = "no";
function custom_showPopup() {
    if (is_filter_open == "no") {
        $('#custom_popup').show();
        is_filter_open = "yes"
    }
    else {
        $('#custom_popup').hide();
        is_filter_open = "no"
    }
}

//Answer 1 View Custom popup

$("#myPopover1").popover({
    html: true,
    placement: "bottom",
    content: function () {
        return $(".answer1content_div").html();
    }
});

var is_filter_open = "no";
function custom_answer1_popup() {
    if (is_filter_open == "no") {
        $('#answer1_popup').show();
        is_filter_open = "yes"
    }
    else {
        $('#answer1_popup').hide();
        is_filter_open = "no"
    }
}

//Answer 2 View Custom popup

$("#myPopover2").popover({
    html: true,
    placement: "bottom",
    content: function () {
        return $(".answer2content_div").html();
    }
});

var is_filter_open = "no";
function custom_answer2_popup() {
    if (is_filter_open == "no") {
        $('#answer2_popup').show();
        is_filter_open = "yes"
    }
    else {
        $('#answer2_popup').hide();
        is_filter_open = "no"
    }
}

function get_user_feedback_data() {
    var crn = $('#crn').val().trim();
    var question_1 = $('#question_1').val();
    var question_2 = $('#question_2').val();
    var suggestion = $('#suggestion').val();
    var from_date = $('#from_date').val();
    var to_date = $('#to_date').val();
    var rating_min_value = $('#min-value').val();
    var rating_max_value = $('#max-value').val();

    if (from_date == "" || to_date == "") {
        $('#err_msg').html('Please fill all the mandatory fields');
    }
    else {
        if (from_date > to_date) {
            $('#err_msg').html("From Date should be less than To Date");
        }
        else {
            $('#err_msg').html("");
            var get_data = $('#search_btn').attr('data-loading-text');
            $('#search_btn').html(get_data);
            $('#search_btn').attr("disabled", true);
            $("#content_div").hide();
            $("#no_data_div").hide();
            $("#top_div").hide();
            $("#bottom_div").hide();
            $("#loading_div").show();
            var send_data = JSON.stringify({
                crn: crn,
                question_1: question_1,
                question_2: question_2,
                suggestion: suggestion,
                from_date: from_date,
                to_date: to_date,
                rating_min_value: rating_min_value,
                rating_max_value: rating_max_value,
                starting_index: start_index,
                record_limit: record_limit
            });
            console.log(send_data);
            $.ajax({
                type: "POST",
                contentType: "application/json",
                datatype: "json",
                data: send_data,
                url: "/Home/GetUserFeedback/",
                success: function (response) {
                    console.log(response);
                    //var response = {
                    //    "StatusCode": 200,
                    //    "Data": [
                    //        {
                    //            "CRN": "123",
                    //            "rating": 2,
                    //            "user_answer1": "Lengthy process,Camera not working",
                    //            "user_answer2": "Application crashed,Camera not working",
                    //            "suggesstion": "Test",
                    //            "Created_on": "04-04-2022 01:02:03"
                    //        },
                    //        {
                    //            "CRN": "123",
                    //            "rating": 2,
                    //            "user_answer1": "Lengthy process,Camera not working",
                    //            "user_answer2": "Application crashed,Camera not working",
                    //            "suggesstion": "Test",
                    //            "Created_on": "04-04-2022 01:02:03"
                    //        }
                    //    ],
                    //"total_records": 10

                    //}
                    try {
                        if (response.StatusCode == 200) {
                            if (response.Data.length == 0) {
                                $('#data_div').hide();
                                $('#loading_div').hide();
                                $('#no_data_div').show();
                            } else {
                                $('#user_feedback_table tbody').empty();
                                var user_feedback_tr_html = "";
                                for (var i = 0; i < response.Data.length; i++) {
                                    user_feedback_tr_html += '<tr><td>' + response.Data[i].CRN + '</td> <td>' + response.Data[i].rating + '</td> <td>' + response.Data[i].user_answer1 + '</td> <td>' + response.Data[i].user_answer2 + '</td> <td>' + response.Data[i].suggesstion + '</td> <td>' + response.Data[i].created_on + '</td></tr>';
                                }
                                $('#user_feedback_table tbody').append(user_feedback_tr_html);
                                $('#data_div').show();
                                $('#loading_div').hide();
                                $('#no_data_div').hide();
                            }
                        }
                        else {
                            $('#data_div').hide();
                            $('#loading_div').hide();
                            $('#no_data_div').show();
                            popup("Something went wrong, Please try again");
                        }
                    }
                    catch (err) {
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
    }
}

function popup(Message) {
    var html = '<div class="modal fade" id="alert_popup" role="dialog" data-keyboard="false" data-backdrop="static"><div class="modal-dialog"><div class="modal-content"><div class="modal-body"><h4 class="text-center">' + Message + '</h4><br><center><button id="modalButton1" type="button" class="btn btn-primary">Okay</button></center></div></div>';
    document.getElementById("alert_popup_div").innerHTML = html;
    $('#alert_popup').modal('show');
    $("#modalButton1").click(function () {
        $('#alert_popup').modal('hide');
    });
}


 