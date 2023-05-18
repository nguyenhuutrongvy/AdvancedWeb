<?php
session_start();

# If the admin is logged in
if (isset($_SESSION["user_id"]) && isset($_SESSION["user_email"])) {
  # Database Connection File
  include "../db_conn.php";

  /**
   * Check if the author id set
   **/
  if (isset($_GET["id"])) {
    /**
     * Get data from GET request nd store it in var
     **/
    $id = $_GET["id"];

    # Simple form Validation
    if (empty($id)) {
      $em = "Error Occurred!";
      header("Location: ../admin.php?error=$em");
      exit();
    } else {
      # DELETE the category from Database
      // $sql  = "DELETE FROM authors
      //          WHERE id=?";
      // $stmt = $conn->prepare($sql);
      // $res  = $stmt->execute([$id]);

      $curl = curl_init();

      curl_setopt_array($curl, [
        CURLOPT_URL => "https://localhost:7007/api/authors/" . $id,
        CURLOPT_CUSTOMREQUEST => "DELETE",
        CURLOPT_RETURNTRANSFER => true,
        CURLOPT_SSL_VERIFYPEER => false,
      ]);

      curl_setopt($curl, CURLOPT_HTTPHEADER, [
        "Content-Type: application/json",
      ]);

      $result = curl_exec($curl);

      curl_close($curl);

      /**
       * If there is no error while ng the data
       **/
      if ($result) {
        # Success message
        $sm = "Xóa thành công!";
        header("Location: ../admin.php?success=$sm");
        exit();
      } else {
        $em = "Có lỗi xảy ra!";
        header("Location: ../admin.php?error=$em");
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
