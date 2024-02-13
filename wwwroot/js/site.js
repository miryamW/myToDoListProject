const uriTasks = 'TasksList';
const uriLogin = 'Login';
const uriUsers = 'Users';
let Tasks = [];
let Users = [];
let token = localStorage.getItem('token');
const tasksDiv = document.getElementById('To-Do-List-CRUD');
const loginDiv = document.getElementById('login-CRUD');
const usersDiv = document.getElementById('users-CRUD');

function checkgetItems() {
    token = localStorage.getItem('token');
    if (token != null) {
        tasksDiv.style.visibility = 'visible';
        loginDiv.style.visibility = 'hidden';
        getItems();
        getUsers();
    }
    else {
        loginDiv.style.visibility = 'visible';
        tasksDiv.style.visibility = 'hidden';
        usersDiv.style.visibility = 'hidden';

    }
}
function getItems() {
    fetch(uriTasks,{  method: 'Get',
    headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json',
        'Authorization':`Bearer ${token}`

    },})
        .then(response => response.json())
        .then(data => _displayItems(data))
        .catch(error => console.error('Unable to get items.', error));
}

function addItem() {
    const adddescriptionTextbox = document.getElementById('add-description');

    const item = {
        id: 0,
        isDone: false,
        description: adddescriptionTextbox.value.trim()
    };

    fetch(uriTasks, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization':`Bearer ${token}`
        },
        body: JSON.stringify(item)
    })
        .then(response => response.json())
        .then(() => {
            getItems();
            adddescriptionTextbox.value = '';
        })
        .catch(error => console.error('Unable to add item.', error));
}

function deleteItem(id) {
    fetch(`${uriTasks}/${id}`, {
        method: 'DELETE',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization':`Bearer ${token}`
        },
    })
        .then(() => getItems())
        .catch(error => console.error('Unable to delete item.', error));
}

function displayEditForm(id) {
    const item = Tasks.find(item => item.id === id);

    document.getElementById('edit-description').value = item.description;
    document.getElementById('edit-id').value = item.id;
    document.getElementById('edit-isDone').checked = item.isDone;
    document.getElementById('editForm').style.display = 'block';
}

function updateItem() {
    const itemId = document.getElementById('edit-id').value;
    const item = {
        id: parseInt(itemId, 10),
        isDone: document.getElementById('edit-isDone').checked,
        description: document.getElementById('edit-description').value.trim()
    };

    fetch(`${uriTasks}/${itemId}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization':`Bearer ${token}`
        },
        body: JSON.stringify(item)
    })
        .then(() => getItems())
        .catch(error => console.error('Unable to update item.', error));

    closeInput();

    return false;
}

function closeInput() {
    document.getElementById('editForm').style.display = 'none';
}

function _displayCount(itemCount) {
    const description = (itemCount === 1) ? 'Task' : 'Tasks';

    document.getElementById('counter').innerText = `${itemCount} ${description}`;
}

function _displayItems(data) {
    const tBody = document.getElementById('Tasks');
    tBody.innerHTML = '';

    _displayCount(data.length);

    const button = document.createElement('button');

    data.forEach(item => {
        let isDoneCheckbox = document.createElement('input');
        isDoneCheckbox.type = 'checkbox';
        isDoneCheckbox.disabled = true;
        isDoneCheckbox.checked = item.isDone;

        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayEditForm(${item.id})`);

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteItem(${item.id})`);

        let tr = tBody.insertRow();

        let td1 = tr.insertCell(0);
        td1.appendChild(isDoneCheckbox);

        let td2 = tr.insertCell(1);
        let textNode = document.createTextNode(item.description);
        td2.appendChild(textNode);

        let td3 = tr.insertCell(2);
        td3.appendChild(editButton);

        let td4 = tr.insertCell(3);
        td4.appendChild(deleteButton);
    });

    Tasks = data;
}

function login(){
    const idTextBox = document.getElementById('id');
    const nameTextBox = document.getElementById('name');
    const passwordTextBox = document.getElementById('password');

    const user = {
        id: parseInt(idTextBox.value.trim()),
        name: nameTextBox.value.trim(),
        password: passwordTextBox.value.trim()
    };

    fetch(uriLogin, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(user)
    })
        .then(response => response.json())
        .then((res) => {
            localStorage.setItem('token',res)
            idTextBox.value = '';
            nameTextBox.value = '';
            passwordTextBox.value = '';
            checkgetItems();
        })
        .catch(error => console.error('Unable to find this user.', error));
}

function getUsers() {
    fetch(uriUsers,{  method: 'Get',
    headers: {
        'Accept': 'application/json',
        'Content-Type': 'application/json',
        'Authorization':`Bearer ${token}`

    },})
        .then(response => response.json())
        .then(data => _displayUsers(data))
        .catch(error => console.error('Unable to get items.', error));
}

function addUser() {
    const addTextbox = document.getElementById('add');
    const user = {
        id: 0,
        name: '',
        password: addTextbox.value.trim()
    };

    fetch(uriUsers, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization':`Bearer ${token}`
        },
        body: JSON.stringify(user)
    })
        .then(response => response.json())
        .then(() => {
            getUsers();
            addTextbox.value = '';
        })
        .catch(error => console.error('Unable to add user.', error));
}

function deleteUser(id) {
    fetch(`${uriUsers}/${id}`, {
        method: 'DELETE',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization':`Bearer ${token}`
        },
    })
        .then(() => getItems_user())
        .catch(error => console.error('Unable to delete user.', error));
}

function displayEditForm_user(id) {
    const user = Users.find(user => user.id === id);

    document.getElementById('edit-id-user').value = user.id;
    document.getElementById('edit-password').value = user.password;
    document.getElementById('edit-name').value = user.name;
    document.getElementById('editForm').style.display = 'block';
}

function updateUser() {
    const userId = document.getElementById('edit-id-user').value;
    const user = {
        id: parseInt(userId, 10),
        name: document.getElementById('edit-name').value.trim(),
        password: document.getElementById('edit-password').value.trim()
    };

    fetch(`${uriUsers}/${itemId}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization':`Bearer ${token}`
        },
        body: JSON.stringify(user)
    })
        .then(() => getItems())
        .catch(error => console.error('Unable to update user.', error));

    closeInput_user();

    return false;
}

function closeInput_user() {
    document.getElementById('editForm-user').style.display = 'none';
}

function _displayCount_user(itemCount) {
    const description = (itemCount === 1) ? 'User' : 'Users';
    document.getElementById('counter-user').innerText = `${itemCount} ${description}`;
}

function _displayUsers(data) {
    usersDiv.style.visibility = 'visible';
    const tBody = document.getElementById('Users');
    tBody.innerHTML = '';
    _displayCount_user(data.length);
    const button = document.createElement('button');
    data.forEach(user => {

        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayEditForm_user(${user.id})`);

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteUser(${user.id})`);

        let tr = tBody.insertRow();

        let td1 = tr.insertCell(1);
        let textNode1 = document.createTextNode(user.name);
        td2.appendChild(textNode1)

        let td2 = tr.insertCell(2);
        let textNode = document.createTextNode(user.password);
        td2.appendChild(textNode);

        let td3 = tr.insertCell(3);
        td3.appendChild(editButton);

        let td4 = tr.insertCell(4);
        td4.appendChild(deleteButton);
    });

    Users  = data;

}

