// Удаление элемента Заказа из таблицы
function deleteItem(itemId) {
    if (confirm('Вы подтверждаете удаление элемента №' + (itemId + 1) + ' ?')) {
        document.getElementById("rowItem_" + itemId).remove();
    }
    useCounter();
}



// Сброс input-полей Модального окна
function clearModal() {
    document.getElementById("ItemNameModal").value = '';
    document.getElementById("ItemQuantityModal").value = '';
    document.getElementById("ItemUnitModal").value = '';

    // Присвоение значения id кнопке в Модальном окне
    var buttonModal = document.getElementById('itemIdModal');
    buttonModal.dataset.id = "-1";
}



// Применение счетчика для всех строк таблицы
function useCounter() {
    var iBlock = document.getElementById("itemBlock");
    var blocks = iBlock.getElementsByTagName("tr");

    for (var i = 0; i < blocks.length; i++) {
        // Обновление id тега tr
        blocks[i].id = 'rowItem_' + i;

        // Обновление id строк таблицы
        var thRow = blocks[i].getElementsByTagName("th");
        thRow[0].textContent = (i + 1);


        // Обновление скрытых полей id элемента
        var inputsHidden = blocks[i].getElementsByClassName("hiddeninputs");

        if (inputsHidden[0] != undefined) {
            inputsHidden[0].id = "ItemId_" + i;
            inputsHidden[0].name = "OrderItems[" + i + "].Id";
        }


        // Обновление всех полей ввода элемента
        var inputsRow = blocks[i].getElementsByClassName("iteminputs");

        if (inputsRow[0] != undefined) {
            inputsRow[0].id = "ItemName_" + i;
            inputsRow[0].name = "OrderItems[" + i + "].Name";
        }

        if (inputsRow[1] != undefined) {
            inputsRow[1].id = "ItemQuantity_" + i;
            inputsRow[1].name = "OrderItems[" + i + "].Quantity";
        }

        if (inputsRow[2] != undefined) {
            inputsRow[2].id = "ItemUnit_" + i;
            inputsRow[2].name = "OrderItems[" + i + "].Unit";
        }


        // Обновление button
        var buttonsRow = blocks[i].getElementsByTagName("button");

        if (buttonsRow[0] != undefined) {
            buttonsRow[0].setAttribute("onClick", "editItem(" + i + ")");
        }

        if (buttonsRow[1] != undefined) {
            buttonsRow[1].setAttribute("onClick", "deleteItem(" + i + ")");
        }


    }
}



// Кнопка Редактировать в таблице. (Редактирование элемента)
function editItem(itemId) {
    // Получение значений элемента из таблицы
    var ItemN = document.getElementById("ItemName_" + itemId).value;
    var ItemQ = document.getElementById("ItemQuantity_" + itemId).value;
    var ItemU = document.getElementById("ItemUnit_" + itemId).value;

    // Присвоение значений элемента полям Модального окна
    document.getElementById("ItemNameModal").value = ItemN;
    document.getElementById("ItemQuantityModal").value = ItemQ;
    document.getElementById("ItemUnitModal").value = ItemU;

    // Присвоение значения id кнопке в Модальном окне
    var buttonModal = document.getElementById('itemIdModal');
    buttonModal.dataset.id = itemId;
}



// Кнопка Сохранить в Модальном окне. (Добавление или Изменение элемента)
function actionItem() {
    // Получение значения id кнопки Модального окна
    var buttonModal = document.getElementById('itemIdModal');
    var itemId = buttonModal.dataset.id;

    // Получение значений из полей Модального окна
    var ItemN = document.getElementById("ItemNameModal").value;
    var ItemQ = document.getElementById("ItemQuantityModal").value;
    var ItemU = document.getElementById("ItemUnitModal").value;

    if (ItemQ == "") {
        ItemQ = 0;
    }

    // Создание нового элемента
    if (itemId == -1) {
        var code = '<tr>' +
            '<th scope="col"></th>' +
            '<td><input value = "' + ItemN + '" type = "text" readonly class="form-control-plaintext form-control-sm iteminputs" /></td>' +
            '<td><input value="' + ItemQ + '" type="number" readonly class="form-control-plaintext form-control-sm iteminputs" /></td>' +
            '<td><input value="' + ItemU + '" type="text" readonly class="form-control-plaintext form-control-sm iteminputs" /></td>' +
            '<td>' +
            '<button type="button" data-bs-toggle="modal" data-bs-target="#staticBackdrop" class="btn btn-sm btn-outline-success">Редактировать</button>' +
            '<button type="button" class="btn btn-sm btn-outline-danger">Удалить</button>' +
            '</td></tr>';
        $('#itemBlock').append(code);
    }

    // Изменение существующего элемента
    if (itemId >= 0) {
        document.getElementById("ItemName_" + itemId).value = ItemN;
        document.getElementById("ItemQuantity_" + itemId).value = ItemQ;
        document.getElementById("ItemUnit_" + itemId).value = ItemU;
    }

    useCounter();
    clearModal();
}