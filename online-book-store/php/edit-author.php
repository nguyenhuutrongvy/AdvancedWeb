<?php
session_start();

# If the admin is logged in
if (isset($_SESSION["user_id"]) && isset($_SESSION["user_email"])) {
  # Database Connection File
  include "../db_conn.php";

  /**
   * Check if author name is submitted
   **/
  if (isset($_POST["author_name"]) && isset($_POST["author_id"])) {
    /**
     * Get data from POST request and store them in var
     **/
    $name = $_POST["author_name"];
    $id = $_POST["author_id"];

    #sImple form Validation
    if (empty($name)) {
      $em = "Tên tác giả là bắt buộc";
      header("Location: ../edit-author.php?error=$em&id=$id");
      exit();
    } else {
      # UPDATE the Database
      // $sql  = "UPDATE authors
      //          SET name=?
      //          WHERE id=?";
      // $stmt = $conn->prepare($sql);
      // $res  = $stmt->execute([$name, $id]);

      $data = [
        "name" => $name,
      ];

      $data_string = json_encode($data, JSON_UNESCAPED_UNICODE);

      $curl = curl_init();

      curl_setopt_array($curl, [
        CURLOPT_URL => "https://localhost:7007/api/authors/" . $id,
        CURLOPT_CUSTOMREQUEST => "PUT",
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
        $sm = "Sửa thành công!";
        header("Location: ../edit-author.php?success=$sm&id=$id");
        exit();
      } else {
        # Error message
        $em = "Có lỗi xảy ra!";
        header("Location: ../edit-author.php?error=$em&id=$id");
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
