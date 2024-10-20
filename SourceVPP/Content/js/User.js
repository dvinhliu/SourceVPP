function fetchUserData() {
    const token = localStorage.getItem('token');
    if (!token) return; // Không làm gì nếu không có token

    fetch('/User/GetUser', {
        method: 'GET',
        headers: {
            'Authorization': 'Bearer ' + token
        }
    })
        .then(response => {
            if (response.status === 401) {
                return refreshToken().then(() => fetchUserData());
            }
            return response.json();
        })
        .then(data => {
            if (data.success) {
                localStorage.setItem('user', JSON.stringify(data.user));
                displayUserName();
            }
            else {
                console.log("Lỗi:", data.message);
            }
        })
        .catch(error => console.error('Có lỗi xảy ra:', error));
}

function refreshToken() {
    return fetch('/User/RefreshToken', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ RefreshToken: localStorage.getItem('refreshToken') }) // Sửa để lấy token đúng cách
    })
        .then(response => {
            if (!response.ok) {
                throw new Error("Không thể làm mới token");
            }
            return response.json();
        })
        .then(data => {
            if (data.success) {
                localStorage.setItem('token', data.token);
                console.log("Token mới đã được lưu.");
            } else {
                console.log("Lỗi khi làm mới token:", data.message);
            }
        });
}

function displayUserName() {
    const userData = localStorage.getItem('user');
    const userInfoElement = document.getElementById('user-info');
    const logoutItem = document.getElementById('logout-item');
    const loginItem = document.getElementById('login-item');

    if (userData) {
        const user = JSON.parse(userData);
        userInfoElement.innerHTML = user.TenTaiKhoan;
        logoutItem.style.display = 'inline';
        loginItem.style.display = 'none';
    } else {
        userInfoElement.innerHTML = '';
        logoutItem.style.display = 'none';
        loginItem.style.display = 'inline';
    }
}

function logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    window.location.reload();
}

const apiKey = "AIzaSyC7PsMUUTQancVCjMl0NnbDVDl0tpWdZDY";

async function signUp(email, password, tenkhachhang, tentaikhoan, gender, dateofbirth) {
    const apiUrl = `https://identitytoolkit.googleapis.com/v1/accounts:signUp?key=${apiKey}`;
    const data = {
        email: email,
        password: password,
        returnSecureToken: true,
    };

    try {
        const response = await fetch(apiUrl, {
            method: "POST",
            headers: {
                "Content-Type": "application/json", // Sửa dấu nháy
            },
            body: JSON.stringify(data),
        });

        const result = await response.json();

        if (result.error) {
            console.error("Lỗi đăng ký: ", result.error.message);
            document.getElementById('registrationError').innerText = "Email này đã tồn tại.";
            document.getElementById('registrationError').style.display = 'block';
            return;
        }

        document.getElementById('registrationError').style.display = 'none';

        await sendEmailVerification(result.idToken);

        await fetchUserDataAndSaveToSQL(result.idToken, tenkhachhang, tentaikhoan, gender, dateofbirth);
    } catch (error) {
        console.error("Lỗi đăng ký: ", error.message);
    }
}

async function sendEmailVerification(idToken) {
    const emailVerificationUrl = `https://identitytoolkit.googleapis.com/v1/accounts:sendOobCode?key=${apiKey}`;
    const verificationData = {
        requestType: "VERIFY_EMAIL",
        idToken: idToken,
    };

    try {
        const response = await fetch(emailVerificationUrl, {
            method: "POST",
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(verificationData),
        });

        const result = await response.json();

        if (result.error) {
            console.error("Lỗi xác thực email: ", result.error.message);
            throw new Error(result.error.message);
        }

        document.getElementById('emailVerificationMessage').innerText = "Một email xác thực đã được gửi. Vui lòng kiểm tra email của bạn.";
        document.getElementById('emailVerificationMessage').style.display = 'block';
    } catch (error) {
        console.error("Lỗi khi gửi email xác thực: ", error.message);
    }
}

async function fetchUserDataAndSaveToSQL(idToken, tenKhachHang, tentaikhoan, gender, dateOfBirth) {
    const userDataUrl = `https://identitytoolkit.googleapis.com/v1/accounts:lookup?key=${apiKey}`;
    const requestData = { idToken: idToken };

    try {
        const response = await fetch(userDataUrl, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(requestData),
        });

        const userData = await response.json();

        if (userData.error) {
            console.error("Lỗi lấy dữ liệu người dùng:", userData.error.message);
            throw new Error(userData.error.message);
        }

        const user = userData.users[0];

        const saveData = {
            MaTaiKhoan: user.localId,
            TenKhachHang: tenKhachHang,
            TenTaiKhoan: tentaikhoan,
            Email: user.email,
            Gender: gender,
            DateOfBirth: dateOfBirth,
            ImageProfile: "",
            MatKhauCap2: "",
        };

        await saveUserDataToSQL(saveData);
    } catch (error) {
        console.error("Có lỗi xảy ra khi lấy dữ liệu người dùng:", error.message);
    }
}

async function saveUserDataToSQL(data) {
    try {
        const response = await fetch('/User/SaveUserData', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(data),
        });

        if (!response.ok) {
            console.error("Lỗi khi lưu dữ liệu:", errorText);
            return;
        }

        const result = await response.json();

        if (result.success) {
            console.log(result.message);
        } else {
            console.error("Có lỗi khi lưu dữ liệu:", result.message);
        }
    } catch (error) {
        console.error("Có lỗi xảy ra khi lưu vào SQL Server:", error);
    }
}

async function SignWithPassword(email, password) {
    const url = `https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key=${apiKey}`;
    const bodydata = {
        email: email,
        password: password,
        returnSecureToken: true,
    };

    try {
        const response = await fetch(url, {
            method: "POST",
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(bodydata),
        });

        const result = await response.json();

        if (result.idToken) {
            document.getElementById('notificationLogin').style.display = 'none';
            const checkemail = await CheckVerifiedEmail(result.idToken);
            if (checkemail) {
                document.getElementById('notificationLogin').style.display = 'none';

                localStorage.setItem('token', result.idToken);

                const token = localStorage.getItem('token');

                const check = await fetch('/User/GetUser', {
                    method: "GET",
                    headers: {
                        'Authorization': 'Bearer ' + token
                    }
                });

                const result2 = await check.json();

                if (result2.success && result2.message === "Người dùng không tìm thấy.") {
                    document.getElementById('notificationLogin').innerText = "Tên tài khoản của bạn hoặc Mật khẩu không đúng, vui lòng thử lại";
                    document.getElementById('notificationLogin').style.display = 'block';
                    return false;
                }
                else {
                    document.getElementById('notificationLogin').style.display = 'none';
                    return true;
                }
            }
            else {
                console.error("Chưa xác thực email");
                document.getElementById('notificationLogin').innerText = "Chưa xác thực email. Vui lòng xác thực email!";
                document.getElementById('notificationLogin').style.display = 'block';
            }
        }
        else {
            console.error("Có lỗi khi lấy dữ liệu signWithPass:", result.message);
            document.getElementById('notificationLogin').innerText = "Tên tài khoản của bạn hoặc Mật khẩu không đúng, vui lòng thử lại";
            document.getElementById('notificationLogin').style.display = 'block';
        }
    }
    catch (error) {
        console.error("Có lỗi xảy ra khi đăng nhập bằng mật khẩu:", error);
    }
}

async function CheckVerifiedEmail(idToken) {
    const url = `https://identitytoolkit.googleapis.com/v1/accounts:lookup?key=${apiKey}`;

    try {
        const response = await fetch(url, {
            method: "POST",
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ idToken: idToken }),
        });

        const result = await response.json();

        if (result.users && result.users.length > 0) {
            // Kiểm tra trạng thái xác thực email
            if (result.users[0].emailVerified) {
                return true; // Email đã được xác thực
            } else {
                return false; // Email chưa được xác thực
            }
        }
        else {
            console.error("Có lỗi khi xác thực email:", result.message);
        }
    }
    catch (error) {
        console.error("Có lỗi xảy ra khi kiểm tra xác thực email: ", error);
    }
}

async function checkUsername(username) {
    try {
        const response = await fetch('/User/CheckUsernameExists', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ username: username }),
        });

        const result = await response.json();
        console.log(result);
        return result.exists;
    } catch (error) {
        console.error("Có lỗi xảy ra khi kiểm tra tên đăng nhập:", error);
        return false; // Hoặc xử lý lỗi theo cách khác
    }
}
