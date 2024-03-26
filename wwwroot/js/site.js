
const uriTasks = 'TasksList';
const uriLogin = 'Login';
const uriUsers = 'Users';
let Tasks = [];
let Users = [];
let token = localStorage.getItem('token');

//Login

const loginWithForm = () => {
    const nameTextBox = document.getElementById('name');
    const passwordTextBox = document.getElementById('password');
    login(nameTextBox.value.trim(), passwordTextBox.value.trim())
    nameTextBox.value = '';
    passwordTextBox.value = '';
}

const login = (name, password) => {
    const user = {
        id: 0,
        name: name,
        password: password,
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
        location.href = './index.html'
        checkgetTasks();
       })
        .catch((error) => {
            console.error('Unable to find this user.', error);
            alert("this user is not exist, try again");
        });
}

const handleCredentialResponse = (response) => {
    const responsePayload = decodeJwtResponse(response.credential);
    login(responsePayload.given_name,responsePayload.email)   
}

const decodeJwtResponse = (jwt) => {
    const [header, payload, signature] = jwt.split('.');
    const decodedPayload = JSON.parse(atob(payload.replace(/_/g, '/').replace(/-/g, '+')));
    return decodedPayload;
}

//Tasks

const checkgetTasks = ()=> {
    token = localStorage.getItem('token');
    if (token != null) {
        getTasks();
        ifGetUsers();
    }
    else {
        location.href = './login.html'
    }
}

const getTasks = () =>
{
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
        _displayTasks(data);

    })
    .catch((error) => {
            console.error('Unable to get Tasks.', error);
            alert("your details got expired, please login again")
            location.href = './login.html'
    });
}

const addTask = () =>{
    const adddescriptionTextbox = document.getElementById('add-description');

    const task = {
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
        body: JSON.stringify(task)
    })
    .then(response => response.json())
    .then(() => {
        getTasks();
        adddescriptionTextbox.value = '';
    })
    .catch((error) => {
        console.error('Unable to add item.', error);
        alert("your details got expired, please login again")
        location.href = './login.html'
    });
}

const deleteTask  = (id) =>{
    fetch(`${uriTasks}/${id}`, {
        method: 'DELETE',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        },
    })
    .then(() => getTasks())
    .catch((error) => {
        console.error('Unable to delete item.', error)
        alert("your details got expired, please login again")
        location.href = './login.html'
    });
}

const displayEditForm = (id) => {
    const task = Tasks.find(item => item.id === id);

    document.getElementById('edit-description').value = task.description;
    document.getElementById('edit-id').value = task.id;
    document.getElementById('edit-isDone').checked = task.isDone;
    document.getElementById('editForm').style.display = 'block';
}

const updateTask = () =>{
    const taskId = document.getElementById('edit-id').value;
    const task = {
        id: parseInt(taskId, 10),
        isDone: document.getElementById('edit-isDone').checked,
        description: document.getElementById('edit-description').value.trim(),
        serId: 0
    };

    fetch(`${uriTasks}/${taskId}`, {
        method: 'PUT',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify(task)
    })
    .then(() => getTasks())
    .catch((error) => {
        console.error('Unable to update item.', error)
        alert("your details got expired, please login again")
        location.href = './login.html'
    });
    closeInput();
    return false;
}

const closeInput = () =>{
    document.getElementById('editForm').style.display = 'none';
}

const _displayCount = (taskCount) =>{
    const description = (taskCount === 1) ? 'Task' : 'Tasks';
    document.getElementById('counter').innerText = `${taskCount} ${description}`;
}

const _displayTasks = (data) =>{
    const tBody = document.getElementById('Tasks');
    tBody.innerHTML = '';
    _displayCount(data.length);
    const button = document.createElement('button');
    data.forEach(item=> 
        {
        let isDoneCheckbox = document.createElement('input');
        isDoneCheckbox.type = 'checkbox';
        isDoneCheckbox.disabled = true;
        isDoneCheckbox.checked = item.isDone;

        let editButton = button.cloneNode(false);
        editButton.innerText = 'Edit';
        editButton.setAttribute('onclick', `displayEditForm(${item.id})`);

        let deleteButton = button.cloneNode(false);
        deleteButton.innerText = 'Delete';
        deleteButton.setAttribute('onclick', `deleteTask(${item.id})`);

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

const updateUserBySelf = () =>{
    
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
        document.getElementById('edit-is-manager').checked = user.userType===1;
        document.getElementById('editForm-user').style.display = 'block';
    })
    .catch((error) => {
        console.error('Unable to update user.', error);
        alert("your details got expired, please login again")
        location.href = './login.html'
    });
}

const ifGetUsers = () =>{
    fetch(uriUsers, {
        method: 'Get',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`
        },
    })
    .then((res) => {
        if (res.status == 200) {
            const usersLink = document.getElementById('users-link');
            usersLink.style.display = 'block';
        }
    })
    .catch((error) => console.error('Unable to get users.', error));
}

//Users

const getUsers = () =>{
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
            _displayUsers(data);
        })
        .catch((error) => console.error('Unable to get users.', error));
}

const addUser = () =>{
    const addNameTextbox = document.getElementById('add-name');
    const addPasswordTextbox = document.getElementById('add-password');
    const isManagerTextbox = document.getElementById('add-is-manager')
    const user = {
        id: 0,
        name: addNameTextbox.value.trim(),
        password: addPasswordTextbox.value.trim(),
        userType: isManagerTextbox?1:0
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
        isManagerTextbox.checked = false;
    })
        .catch(error => console.error('Unable to add user.', error));
}

const deleteUser = (id) =>{
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

const displayEditForm_user = (id) =>{

    const user = Users.find(user => user.id === id);

    document.getElementById('edit-id-user').value = user.id;
    document.getElementById('edit-password').value = user.password;
    document.getElementById('edit-name').value = user.name;
    document.getElementById('edit-is-manager').checked = user.userType == 1;
    document.getElementById('editForm-user').style.display = 'block';
}

const updateUser = () =>{
    let userId = document.getElementById('edit-id-user').value;
    let userType = 0;
    if (document.getElementById('edit-is-manager').checked)
        userType = 1;
    const user = {
        id: parseInt(userId, 10),
        name: document.getElementById('edit-name').value.trim(),
        password: document.getElementById('edit-password').value.trim(),
        userType: userType
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

const closeInput_user = () =>{
    document.getElementById('editForm-user').style.display = 'none';
}

const _displayUsers = (data) =>{
    const tBody = document.getElementById('Users');
    tBody.innerHTML = '';
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

