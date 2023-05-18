<?php
session_start();

# If the admin is logged in
if (isset($_SESSION["user_id"]) && isset($_SESSION["user_email"])) {
  # Database Connection File
  include "../db_conn.php";

  /**
   * Check if category name is submitted
   **/
  if (isset($_POST["category_name"])) {
    /**
     * Get data from POST request and store it in var
     **/
    $name = $_POST["category_name"];

    #sImple form Validation
    if (empty($name)) {
      $em = "The category name is required";
      header("Location: ../add-category.php?error=$em");
      exit();
    } else {
      # Insert Into Database
      // $sql  = "INSERT INTO categories (name)
      //          VALUES (?)";
      // $stmt = $conn->prepare($sql);
      // $res  = $stmt->execute([$name]);

	  $data = [
		"name" => $name
	  ];

      $data_string = json_encode($data, JSON_UNESCAPED_UNICODE);

      $curl = curl_init();

      curl_setopt_array($curl, [
        CURLOPT_URL => "https://localhost:7007/api/categories",
        CURLOPT_CUSTOMREQUEST => "POST",
        CURLOPT_POSTFIELDS => $data_string,
        CURLOPT_RETURNTRANSFER => true,
        CURLOPT_SSL_VERIFYPEER => false,
      ]);

      curl_setopt($curl, CURLOPT_HTTPHEADER, [
        "Content-Type: application/json",
        "Content-Length: " . strlen($data_string),
      ]);

      $result = curl_exec($curl);

      curl_close($curl);

      /**
       * If there is no error while inserting the data
       **/
      if ($result) {
        # Success message
        $sm = "Thêm thành công!";
        header("Location: ../add-category.php?success=$sm");
        exit();
      } else {
        # Error message
        $em = "Có lỗi xảy ra!";
        header("Location: ../add-category.php?error=$em");
        exit();
      }
    }
  } else {
    header("Location: ../admin.php");
    exit();
  }
} else {
  header("Location: ../login.php");
  exit();
}
