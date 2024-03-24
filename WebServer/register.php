<?php
header("Access-Control-Allow-Origin: *");

use PHPMailer\PHPMailer\PHPMailer;
            use PHPMailer\PHPMailer\Exception;

// Зчитування параметрів підключення з JSON-файлу
$config = json_decode(file_get_contents('config.json'), true);

// Параметри підключення до бази даних
$servername = $config['servername'];
$username = $config['username'];
$password = $config['password'];
$dbname = $config['dbname'];

// Інформація для відправки листів
$email_credentials = $config['email_credentials'];
$sender_email = $email_credentials['email'];
$sender_password = $email_credentials['password'];

// Створення з'єднання з базою даних
$conn = new mysqli($servername, $username, $password, $dbname);

// Перевірка з'єднання
if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
}

// Отримання даних з POST-запиту
$data = json_decode(file_get_contents('php://input'), true);

$username = $data['username'] ?? null;
$email = $data['email'] ?? null;
$password = $data['password'] ?? null;

// Перевірка наявності всіх даних
if ($username !== null && $password !== null && $email !== null) {
    // Генерація токену
    $token = md5(uniqid($username, true));

    // Перевірка на пусті значення
    if (!empty($username) && !empty($password) && !empty($email)) {
        // Хешування пароля
        $password_hash = password_hash($password, PASSWORD_DEFAULT);

        // Підготовлений запит з параметрами
        $sql = "INSERT INTO users (username, password_hash, email, verification_token) VALUES (?, ?, ?, ?)";
        $stmt = $conn->prepare($sql);

        // Прив'язка параметрів
        $stmt->bind_param("ssss", $username, $password_hash, $email, $token);

        // Виконання запиту
        if ($stmt->execute()){
            // Створення об'єкта PHPMailer
            require 'PHPMailer/src/Exception.php';
            require 'PHPMailer/src/PHPMailer.php';
            require 'PHPMailer/src/SMTP.php';

            $mail = new PHPMailer(true);
            
            // Відправка листа
            try {
                // Налаштування сервера SMTP
                $mail->isSMTP();
                $mail->Host = 'smtp.gmail.com';
                $mail->SMTPAuth = true;
                $mail->Username = $sender_email; // Ваша електронна адреса Gmail
                $mail->Password = $sender_password; // Ваш пароль Gmail
                $mail->SMTPSecure = 'tls';
                $mail->Port = 587;
                
                // Налаштування відправника і отримувача
                $mail->setFrom($sender_email, 'Danger Path team'); // Ваша електронна адреса та ім'я
                $mail->addAddress($email); // Електронна адреса отримувача
                $mail->addReplyTo($sender_email, 'Danger Path team'); // Ваша електронна адреса та ім'я
                
                // Встановлення теми і тіла листа
                $mail->Subject = '=?UTF-8?B?'.base64_encode('Підтвердження адреси електронної пошти').'?=';
                $mail->Body = 'Будь ласка, перейдіть за посиланням для підтвердження адреси електронної пошти: http://dangerpath.000webhostapp.com/verification.php?token=' . $token;
                
                // Відправка листа
                $mail->send();
                echo json_encode(array("success" => true, "message" => "New record created successfully. Email sent!"));
            } catch (Exception $e) {
                echo json_encode(array("success" => false, "message" => "Error: {$mail->ErrorInfo}"));
            }
        } else {
            echo json_encode(array("success" => false, "message" => "Error: " . $sql . "<br>" . $conn->error));
        }

        // Закриття підготовленого запиту і з'єднання
        $stmt->close();
    } else {
        // Виведення повідомлення про помилку, якщо не всі дані були отримані
        echo json_encode(array("success" => false, "message" => "Empty fields"));
    }
} else {
    // Виведення повідомлення про помилку, якщо не всі дані були отримані
    echo json_encode(array("success" => false, "message" => "Not all required data was received"));
}

// Закриття з'єднання
$conn->close();
?>