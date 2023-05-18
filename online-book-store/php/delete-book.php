<?php
session_start();

# If the admin is logged in
if (isset($_SESSION["user_id"]) && isset($_SESSION["user_email"])) {
  # Database Connection File
  include "../db_conn.php";

  /**
   * Check if the book id set
   **/
  if (isset($_GET["id"])) {
    /**
     * Get data from GET request and store it in var
     **/
    $id = $_GET["id"];

    # Simple form Validation
    if (empty($id)) {
      $em = "Có lỗi xảy ra!";
      header("Location: ../admin.php?error=$em");
      exit();
    } else {
      # GET book from Database
      //   $sql2 = "SELECT * FROM books
      // 		          WHERE id=?";
      //   $stmt2 = $conn->prepare($sql2);
      //   $stmt2->execute([$id]);
      //   $the_book = $stmt2->fetch();

      //   if ($stmt2->rowCount() > 0) {
      # DELETE the book from Database
      // $sql = "DELETE FROM books
      // 		         WHERE id=?";
      // $stmt = $conn->prepare($sql);
      // $res = $stmt->execute([$id]);

      $curl = curl_init();

      curl_setopt_array($curl, [
        CURLOPT_URL => "https://localhost:7007/api/books/" . $id,
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
       * If there is no error while Deleting the data
       **/
      if ($result) {
        include "func-book.php";
        $book = get_book($id);
        # Delete the current book_cover and the file
        $cover = $book["result"]["cover"];
        $file = $book["result"]["file"];
        $c_b_c = "../uploads/cover/$cover";
        $c_f = "../uploads/files/$cover";

        unlink($c_b_c);
        unlink($c_f);

        # Success message
        $sm = "Xóa thành công!";
        header("Location: ../admin.php?success=$sm");
        exit();
      } /*else {
          # Error message
          $em = "Unknown Error Occurred!";
          header("Location: ../admin.php?error=$em");
          exit();
        }
      }*/ else {
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
