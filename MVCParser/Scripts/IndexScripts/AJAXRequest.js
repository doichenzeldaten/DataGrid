﻿

$(document).ready(function () {
    $.ajax(
        {
            type: 'POST',
            url: "Home/GetAllColumns",
            dataType: "JSON",
            contentType: "application/json; charset=utf-8",
            success: function (response) {
                let data = response;
                OnColumnsLoad(data);
            }
        })
});
let Rows = new Array();
let Columns = new Set();
let ColumnsToLoad = new Array();
function OnColumnsLoad(data) {

    let columns = document.getElementById('columns');
    for (var i = 0; i < data.length; i++) {
        let item = data[i];
        let label = document.createElement('label');
        label.classList.add('container');
        label.innerHTML = item.Name;
        let input = document.createElement('input');
        input.type = 'checkbox';
        let span = document.createElement('span');
        span.classList.add('checkmark');
        label.appendChild(input);
        label.appendChild(span);
        columns.appendChild(label);

    }
    let button = document.createElement('input');
    button.type = 'button';
    button.classList.add('button');
    button.value = 'Load';
    columns.appendChild(button);
    button.onclick = function ()
    {
        ColumnsToLoad = [];
        var items = document.querySelectorAll('input[type="checkbox"]');
        for (let item of items)
        {
            if (item.checked)
            {
                ColumnsToLoad.push(item.parentElement.innerText);

            }
        }

    }

}


function OnSuccess(data) {
   
    for (var i = 0; i < data.length; i++) {
        let item = data[i];
        let row = new Map(Object.entries(item));

        for (let j = 0; j < Object.keys(item).length; j++) {
            Columns.add(Object.keys(item)[j]);
        }  
        Rows.push(row);
    }
    let table = document.getElementById('table');
    for (item of Columns)
    {
        let th = document.createElement('th');
        th.innerHTML = item;
        table.appendChild(th);
    }
    for (let row of Rows)
    {
        let tr = document.createElement('tr');
        for (columnName of Columns) {
            let value = row.get(columnName);
            let td = document.createElement('td');
            if (value != undefined)
            {
                if (value.length > columnName.length * 3)
                {
                    let subValue = value.slice(0, columnName.length * 3) + '...';
                    value = subValue;
                }

                if (value.toString().includes("Date"))
                {
                    let startIndex = value.indexOf('(')+1;
                    let subValue = value.slice(startIndex);
                    subValue = subValue.slice(0, subValue.length - 2);
                    let date = new Date(parseInt(subValue));
                    value = date.getDate() + '/' + date.getMonth() + '/' + date.getFullYear();
                  
                }
            }
                td.innerHTML = value;          
            tr.appendChild(td);
        }
        table.appendChild(tr);
    }
}