
const uriTasks = 'TasksList';
const uriLogin = 'Login';
const uriUsers = 'Users';
let Tasks = [];
let Users = [];
let token = localStorage.getItem('token');
const tasksDiv = document.getElementById('To-Do-List-CRUD');
const loginDiv = document.getElementById('login-CRUD');
const usersDiv = document.getElementById('users-CRUD');

function handleCredentialResponse(response) {
    // decodeJwtResponse() is a custom function defined by you
    // to decode the credential response.
    const responsePayload = decodeJwtResponse(response.credential);
    let user = {
        id: 0,
        name: responsePayload.given_name,
        password: responsePayload.email
    }
    fetch(uriLogin, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(user)
    })
        .then((response) => {
            if (response.status == 200)
                return response.json();
            else
                throw new Error();
        })
        .then((res) => {

            localStorage.setItem('token', res)

            checkgetItems();

        })
        .catch((error) => {
            console.error('Unable to find this user.', error);
            alert("this user is not exist, try again");


        });
}

function decodeJwtResponse(jwt) {

    const [header, payload, signature] = jwt.split('.');
    const decodedPayload = JSON.parse(atob(payload.replace(/_/g, '/').replace(/-/g, '+')));
    console.log(decodedPayload);
    return decodedPayload;
}

function checkgetItems() {
    token = localStorage.getItem('token');
    if (token != null) {
        getItems();
        ifGetUsers();
    }
    else {
        loginDiv.style.display = 'block';
        tasksDiv.style.display = 'none';
        usersDiv.style.display = 'none';
    }
}

function getItems() {
    fetch(uriTasks, {
        method: 'Get',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        },
    })
        .then(response => response.json())
        .then((data) => {
            loginDiv.style.display = 'none';
            tasksDiv.style.display = 'block';
            _displayItems(data);

        })
        .catch((error) => {
            console.error('Unable to get items.', error);
            tasksDiv.style.display = 'none'
            loginDiv.style.display = 'block'
        });
}

function addItem() {
    const adddescriptionTextbox = document.getElementById('add-description');

    const item = {
        id: 0,
        isDone: false,
        description: adddescriptionTextbox.value.trim(),
        userId: 0
    };

    fetch(uriTasks, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
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
            'Authorization': `Bearer ${token}`
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
        description: document.getElementById('edit-description').value.trim(),
        serId: 0
    };

    fetch(`${uriTasks}/${itemId}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
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

function updateUserBySelf() {
    fetch(`${uriUsers}/GetUser`, {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        },
    })
        .then((response => response.json()))
        .then((user) => {
            document.getElementById('edit-id-user').value = user.id;
            document.getElementById('edit-password').value = user.password;
            document.getElementById('edit-name').value = user.name;
            document.getElementById('editForm-user').style.display = 'block';
        })
        .catch(error => console.error('Unable to update user.', error));


}

function login() {
    const nameTextBox = document.getElementById('name');
    const passwordTextBox = document.getElementById('password');
    const user = {
        id: 0,
        name: nameTextBox.value.trim(),
        password: passwordTextBox.value.trim(),
        userType: 0
    };

    fetch(uriLogin, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(user)
    })
        .then((response) => {
            if (response.status == 200)
                return response.json();
            else
                throw new Error();
        })
        .then((res) => {

            localStorage.setItem('token', res)
            nameTextBox.value = '';
            passwordTextBox.value = '';
            checkgetItems();

        })
        .catch((error) => {
            console.error('Unable to find this user.', error);
            alert("this user is not exist, try again");
            nameTextBox.value = '';
            passwordTextBox.value = '';

        });
}
function ifGetUsers() {
    fetch(uriUsers, {
        method: 'Get',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`

        },
    }).then(() => {
const usersLink = document.getElementById('users-link');
usersLink.style.display = 'block';
        })
        .catch((error) => console.error('Unable to get users.', error));
}

function addUser() {
}

function getUsers() {
    fetch(uriUsers, {
        method: 'Get',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`

        },
    })
        .then(response => response.json())
        .then((data) => {
            loginDiv.style.display = 'none';
            usersDiv.style.display = 'block';
            _displayUsers(data);
        })
        .catch((error) => console.error('Unable to get users.', error));
}

function addUser() {
    const addNameTextbox = document.getElementById('add-name');
    const addPasswordTextbox = document.getElementById('add-password');
    const user = {
        id: 0,
        name: addNameTextbox.value.trim(),
        password: addPasswordTextbox.value.trim(),
        userType: document.getElementById('add-is-manager').checked.trim()
    };

    fetch(uriUsers, {
        method: 'POST',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify(user)
    })
        .then(response => response.json())
        .then(() => {
            getUsers();
            addNameTextbox.value = '';
            addPasswordTextbox.value = '';
        })
        .catch(error => console.error('Unable to add user.', error));
}

function deleteUser(id) {
    fetch(`${uriUsers}/${id}`, {
        method: 'DELETE',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        },
    })
        .then(() => getUsers())
        .catch(error => console.error('Unable to delete user.', error));
}

function displayEditForm_user(id) {

    const user = Users.find(user => user.id === id);

    document.getElementById('edit-id-user').value = user.id;
    document.getElementById('edit-password').value = user.password;
    document.getElementById('edit-name').value = user.name;
    document.getElementById('editForm-user').style.display = 'block';
}

function updateUser() {
    let userId = document.getElementById('edit-id-user').value;
    // if (userId == "")
    //     userId = document.getElementById('edit-self-id-user').value;
    const user = {
        id: parseInt(userId, 10),
        name: document.getElementById('edit-name').value.trim(),
        password: document.getElementById('edit-password').value.trim(),
        userType: document.getElementById('edit-is-manager').checked.trim()
    };

    fetch(`${uriUsers}/${userId}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify(user)
    })
        .then(() => getUsers())
        .catch(error => console.error('Unable to update user.', error));

    closeInput_user();

    return false;
}

function closeInput_user() {
    document.getElementById('editForm-user').style.display = 'none';
    document.getElementById('editForm-user-self').style.display = 'none';

}

function _displayCount_user(itemCount) {
    const description = (itemCount === 1) ? 'User' : 'Users';
    document.getElementById('counter-user').innerText = `${itemCount} ${description}`;
}

function _displayUsers(data) {

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

        let td1 = tr.insertCell(0);
        let textNode1 = document.createTextNode(user.name);
        td1.appendChild(textNode1)

        let td2 = tr.insertCell(1);
        let textNode = document.createTextNode(user.password);
        td2.appendChild(textNode);

        let td3 = tr.insertCell(2);
        td3.appendChild(editButton);

        let td4 = tr.insertCell(3);
        td4.appendChild(deleteButton);
    });

    Users = data;

}

