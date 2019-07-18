// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
$(function () {
    $("#countries").change(function () {
        var countryId = $(this).val();

        if (countryId) {
            $.ajax({
                url: "/Ajax/LoadCitiesByCountryId?countryId=" + countryId,
                type: "POST",
                success: function (res) {
                    $("#CityId").html(res);
                    $("#CityId").prepend("<option value=''>Select city</option>");
                }
            });
        }
    });
});
