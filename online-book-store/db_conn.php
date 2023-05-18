<?php

# Server name
$sName = "localhost";
# User name
$uName = "root";
# Password
$pass = "";

# Database name
$db_name = "online_book_store_db";

/**
Creating database connection 
useing The PHP Data Objects (PDO)
**/
try {
  $conn = new PDO("mysql:host=$sName;dbname=$db_name", $uName, $pass);
  $conn->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
} catch (PDOException $e) {
  echo "Connection failed : " . $e->getMessage();
}
