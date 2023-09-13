
var frmleavetype = document.getElementById('frmleavetype');

var isEdit = false;

async function getLeaveTypes(tbl_container) {

    tbl_container.innerHTML = '';

    let url = "/GettAllLeaveType";

    fetch(url, {
        method: "GET"
    }).then((response) => { return response.json(); })
        .then((d) => {
            d.data.forEach(leavetype => RenderAccordion(tbl_container, leavetype));
        })
        .catch((error) => {
            console.log(error);
        });
}


function RenderAccordion(tbl_container, leavetype) {

    let div = document.createElement('div');

    //ModalItem('modalleavetype', { id: ${ leavetype.id }, name: '${leavetype.name}', name: '${leavetype.name}', description: '${leavetype.description}'}, true)

    //console.log(product);


    div.className = 'accordion-item';
    div.innerHTML = `
    <h4 class="accordion-header" id="${leavetype.id}">
                        <button class="accordion-button" type="button" data-bs-toggle="collapse" data-bs-target="#collapse${leavetype.id}" aria-expanded="true" aria-controls="collapse${leavetype.id}">
                          <div class="form-check">
                      <input class="form-check-input" type="checkbox" value="${leavetype.id}">
                     
                    </div>
                        ${leavetype.name}
                        </button>
    </h4>
     <div id="collapse${leavetype.id}" class="accordion-collapse collapse ${leavetype.id === 1 ? 'show' : ''}" aria-labelledby="${leavetype.id}" data-bs-parent="#accordionExample">
                        <div class="accordion-body">
                            <p>
                            ${leavetype.description}
                            </p>
                            <div class="d-flex justify-content-center align-items-center">
        <div class="flex align-items-center list-user-action">
            <a class="btn btn-sm btn-icon btn-warning" data-bs-toggle="tooltip" onclick="ModalItem('modalleavetype', { id: ${leavetype.id}, name: '${leavetype.name}', description: '${leavetype.description}'}, true)" data-bs-placement="top" data-original-title="Edit"  aria-label="Edit" data-bs-original-title="Edit">
                <span class="btn-inner" style="display:flex">
                    <svg class="icon-20" width="20" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                        <path d="M11.4925 2.78906H7.75349C4.67849 2.78906 2.75049 4.96606 2.75049 8.04806V16.3621C2.75049 19.4441 4.66949 21.6211 7.75349 21.6211H16.5775C19.6625 21.6211 21.5815 19.4441 21.5815 16.3621V12.3341" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round"></path>
                        <path fill-rule="evenodd" clip-rule="evenodd" d="M8.82812 10.921L16.3011 3.44799C17.2321 2.51799 18.7411 2.51799 19.6721 3.44799L20.8891 4.66499C21.8201 5.59599 21.8201 7.10599 20.8891 8.03599L13.3801 15.545C12.9731 15.952 12.4211 16.181 11.8451 16.181H8.09912L8.19312 12.401C8.20712 11.845 8.43412 11.315 8.82812 10.921Z" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round"></path>
                        <path d="M15.1655 4.60254L19.7315 9.16854" stroke="currentColor" stroke-width="1.5" stroke-linecap="round" stroke-linejoin="round"></path>
                    </svg>
                </span>
            </a>
      
        </div>
    </div>
                        </div>
                    </div>
    `;


    tbl_container.appendChild(div);
}

async function ModalItem(modelid, leavetype, isedit) {

    try {

        var modelElm = document.getElementById(modelid);

        var myModal = new bootstrap.Modal(modelElm, {
            keyboard: false
        })

        if (leavetype != null) {
            document.querySelector('.modal-title').textContent = 'Edit Leave Type';
            document.getElementById('id').value = leavetype.id;
            document.getElementById('Name').value = leavetype.name;
            document.getElementById('Description').value = leavetype.description;
        }
        else {
            document.getElementById('Name').value = null
            document.getElementById('Description').value = null
            document.querySelector('.modal-title').textContent = 'Add Leave Type';
        }

        if (isedit) {
            document.getElementById('EditSubmit').hidden = false;
            document.getElementById('AddSubmit').hidden = true;
            isEdit = true;
        }
        else {
            document.getElementById('EditSubmit').hidden = true;
            document.getElementById('AddSubmit').hidden = false;
        }

        myModal.show();

    } catch (e) {
        console.log(e);
    }
}



async function DeleteMulti() {

    var listids = [];

    let formdata = new FormData();

    var checklist = document.querySelectorAll('input[type="checkbox"]');


    checklist.forEach((checkitem) => {
        if (checkitem.checked) {
            listids.push(parseInt(checkitem.value));
        }
    })

    listids.forEach((id) => {
        formdata.append("Ids", id);
    });

    let url = '/DeleteMultipleLeaveType';

    if (listids.length > 0) {
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
                fetch(url, {
                    method: "POST",
                    body: formdata
                }).then((resp) => {
                    if (resp.status === 200) {
                        Swal.fire(
                            'Deleted!',
                            'Your file has been deleted.',
                            'success'
                        )
                        document.querySelector('#accordionExample').innerHTML = '';
                        Showalert(false, true)
                        getLeaveTypes(document.querySelector('#accordionExample'));
                    }
                }).catch((error) => { console.log(error); })
            }
        })
    }
    else {
        Swal.fire({
            title: 'Error!!',
            text: "You did not select any item to delete",
            icon: 'error',
        })
    }



}

function Showalert(ispost, isdelete) {
    var div = document.createElement('div');
    div.className = 'alert alert-left alert-success alert-dismissible fade show mt-2';
    div.setAttribute('role', 'alert');

    var button = document.createElement('button');
    button.className = 'btn-close btn-close';
    button.setAttribute('data-bs-dismiss', 'alert');
    button.setAttribute('aria-label', 'Close');

    var span = document.createElement('span');

    var p = document.createElement('p');

    if (ispost && isdelete === false) {
        p.textContent = 'Addition completed successfully';
    }
    else if (ispost === false && isdelete === false) {
        p.textContent = 'The data has been updated successfully';
    }
    else {
        p.textContent = 'The items you selected have been deleted successfully';

    }

    button.appendChild(span);
    div.appendChild(button);
    div.appendChild(p);

    document.querySelector('#bigdiv').appendChild(div);

}



async function PostLeaveTypebyForm(formdata) {

    let url = "/CreateLeaveType";

    fetch(url, {
        method: "POST",
        body: formdata
    }).then((response) => {

        if ([200, 201].indexOf(response.status) !== -1) {
            response.json().then((product) => {
                document.getElementById('btnClose').click();
                frmleavetype.reset();
                Showalert(true, false);
                getLeaveTypes(document.querySelector('#accordionExample'));
            });
        }
        else {
            response.json().then(error => {
            })
        }
    })
        .catch((error) => console.error(error));
}


async function PutLeaveTypbyForm(formdata) {

    let url = `/EditLeaveType`;

    fetch(url, {
        method: "POST",
        body: formdata
    }).then(async (response) => {

        if ([200, 201].indexOf(response.status) !== -1) {
            var product = await response.json()

            console.log(product);

            frmleavetype.reset();
            document.getElementById('btnClose').click();
            Showalert(false, false);
            getLeaveTypes(document.querySelector('#accordionExample'));

        }
        else {
            response.json().then(error => {
            })
        }
    })
        .catch((error) => console.error(error));
}




frmleavetype.addEventListener('submit', (event) => {

    event.preventDefault();

    if (frmleavetype.checkValidity() === false) {
        event.preventDefault();
        event.stopPropagation();
    }
    else {
        let formdata = new FormData();
        formdata.append("Name", document.getElementById('Name').value);
        formdata.append("Description", document.getElementById('Description').value);


        if (!isEdit) {
            //formdata.append("Id", null);
            PostLeaveTypebyForm(formdata);
            frmleavetype.classList.remove('was-validated')
        }
        else {
            formdata.append("Id", frmleavetype.id.value);
            frmleavetype.classList.remove('was-validated')
            PutLeaveTypbyForm(formdata);
        }

    }


});


getLeaveTypes(document.querySelector('#accordionExample'));

