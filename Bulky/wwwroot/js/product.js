var dataTable;
$(document).ready(function () {
    loadDataTable()
})

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        "ajax": { url:'/admin/product/getall' },
        "columns": [
            { data: 'title', "width": "25%" },
            { data: 'isbn', "width": "15%" },
            { data: 'price', "width": "10%" },
            { data: 'author', "width": "15%" },
            { data: 'category.name', "width": "15%" },
            {
                data: 'id',
                "render": function (data) {
                   return `<div class="btn-group w-75" role="group"">
                                <a href="/admin/product/upsert?id=${data}" class="btn btn-primary"><i class="bi bi-pencil"></i>Edit</a>
                                <a onClick=Delete("/admin/product/delete?id=${data}") class="btn btn-danger mx-2"><i class="bi bi-fill-trash"></i>Delete</a>
                           </div>`
                }
            }
    
        ]


    });
}


function Delete(url) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: "DELETE",
                success: function (data) {
                    if (data.success = true) {
                        dataTable.ajax.reload();
                        toastr.success(data.message)
                    }
                }
            });
        }
    })
}