
const uriLogin = 'Login';
let Tasks = [];
let Users = [];
let token = localStorage.getItem('token');


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
       })
        .catch((error) => {
            console.error('Unable to find this user.', error);
            alert("this user is not exist, try again");
        });
}

function handleCredentialResponse  (response)  {
    const responsePayload = decodeJwtResponse(response.credential);
    login(responsePayload.given_name,responsePayload.email)   
}

function decodeJwtResponse (jwt)  {
    const [header, payload, signature] = jwt.split('.');
    const decodedPayload = JSON.parse(atob(payload.replace(/_/g, '/').replace(/-/g, '+')));
    return decodedPayload;
}