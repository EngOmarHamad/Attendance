function PaginationData(url, data, RenderDataItem, LoaderHtml = null, NoDataHtml = null) {
    var totalPages = 0;
    if (LoaderHtml == null) {
        LoaderHtml = `<tr><td colspan="7"><div class="d-flex justify-content-center align-items-center"  style="height:${($("#SizeSelect").val() * 30)}px;width:100%"> <div class="spinner-border text-primary" role="status"><span class="visually-hidden">Loading...</span></div></div></td></tr>`
    }
    if (NoDataHtml == null) {
        NoDataHtml = `<tr><td colspan="7"><div class="d-flex justify-content-center align-items-center flex-column"><i class="bx bxs-data text-secondary" style="font-size:120px"></i><p>No Data Matched</p></div></td></tr>`
    }
    $("#Tblbady").html(LoaderHtml);
    console.log(data)
    $.ajax({
        url: url,
        data: data,
        success: function (result) {
            console.log(result)
            totalPages = result.totalPages;
            $(`#Tblbady`).html('');
            if (result.data.length > 0)
                $.each(result.data, function (index, dataitem) {
                    console.log(dataitem)
                    RenderDataItem(dataitem);
                });
            else
                $('#Tblbady').html(NoDataHtml);

            if (result.previousPage === null) $("#previousPage").addClass("disabled"); else $("#previousPage").removeClass("disabled");

            if ((data.CurPage != 1 && result.totalPages != 0)) $("#firstPage").removeClass("disabled"); else $("#firstPage").addClass("disabled");

            if (result.nextPage === null) $("#nexPage").addClass("disabled"); else $("#nexPage").removeClass("disabled");

            if ((data.CurPage != result.totalPages && result.totalPages != 0)) $("#lastPage").removeClass("disabled"); else $("#lastPage").addClass("disabled");

            $("#CurPageOfTotalPages").html(`<span> ${data.CurPage}</span> of <span>${result.totalPages}</span > `)
            $("#RecordsFromTo").html(`records from ${result.firstRowOnPage} to  ${result.lastRowOnPage} of ${result.totalCount} `)


        }
    })

    return totalPages;
}

