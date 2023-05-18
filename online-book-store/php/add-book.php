<?php
session_start();

# If the admin is logged in
if (isset($_SESSION["user_id"]) && isset($_SESSION["user_email"])) {
  # Database Connection File
  include "../db_conn.php";

  # Validation helper function
  include "func-validation.php";

  # File Upload helper function
  include "func-file-upload.php";

  /**
   * If all Input field are filled
   **/
  if (
    isset($_POST["book_title"]) &&
    isset($_POST["book_description"]) &&
    isset($_POST["book_author"]) &&
    isset($_POST["book_category"]) &&
    isset($_FILES["book_cover"]) &&
    isset($_FILES["file"])
  ) {
    /**
     * Get data from POST request and store them in var
     **/
    $title = $_POST["book_title"];
    $description = $_POST["book_description"];
    $author = $_POST["book_author"];
    $category = $_POST["book_category"];

    # Making URL data format
    $user_input =
      "title=" .
      $title .
      "&category_id=" .
      $category .
      "&desc=" .
      $description .
      "&author_id=" .
      $author;

    # Simple form Validation

    $text = "Book title";
    $location = "../add-book.php";
    $ms = "error";
    is_empty($title, $text, $location, $ms, $user_input);

    $text = "Book description";
    $location = "../add-book.php";
    $ms = "error";
    is_empty($description, $text, $location, $ms, $user_input);

    $text = "Book author";
    $location = "../add-book.php";
    $ms = "error";
    is_empty($author, $text, $location, $ms, $user_input);

    $text = "Book category";
    $location = "../add-book.php";
    $ms = "error";
    is_empty($category, $text, $location, $ms, $user_input);

    # Book cover Uploading
    $allowed_image_exs = ["jpg", "jpeg", "png"];
    $path = "cover";
    $book_cover = upload_file($_FILES["book_cover"], $allowed_image_exs, $path);

    /**
     * If error occurred while uploading the book cover
     **/
    if ($book_cover["status"] == "error") {
      $em = $book_cover["data"];

      /**
       * Redirect to '../add-book.php' and passing error message & user_input
       **/
      header("Location: ../add-book.php?error=$em&$user_input");
      exit();
    } else {
      # File Uploading
      $allowed_file_exs = ["pdf", "docx", "pptx"];
      $path = "files";
      $file = upload_file($_FILES["file"], $allowed_file_exs, $path);

      /**
       * If error occurred while uploading the file
       **/
      if ($file["status"] == "error") {
        $em = $file["data"];

        /**
         * Redirect to '../add-book.php' and passing error message & user_input
         **/
        header("Location: ../add-book.php?error=$em&$user_input");
        exit();
      } else {
        /**
         * Getting the new file name and book cover name
         **/
        $file_URL = $file["data"];
        $book_cover_URL = $book_cover["data"];

        # Insert the data into database
        // $sql  = "INSERT INTO books (title,
        //                             author_id,
        //                             description,
        //                             category_id,
        //                             cover,
        //                             file)
        //          VALUES (?,?,?,?,?,?)";
        // $stmt = $conn->prepare($sql);
        // $res  = $stmt->execute([$title, $author, $description, $category, $book_cover_URL, $file_URL]);

        $data = [
          "title" => $title,
          "description" => $description,
          "cover" => $book_cover_URL,
          "file" => $file_URL,
          "authorId" => (int) $author,
          "categoryId" => (int) $category,
        ];

        $data_string = json_encode($data, JSON_UNESCAPED_UNICODE);

        $curl = curl_init();

        curl_setopt_array($curl, [
          CURLOPT_URL => "https://localhost:7007/api/books",
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

        // print_r($data_string);
        // print_r($result);

        /**
         * If there is no error while inserting the data
         **/
        if ($result) {
          # Success message
          $sm = "Thêm thành công!";
          header("Location: ../add-book.php?success=$sm");
          exit();
        } else {
          # Error message
          $em = "Có lỗi xảy ra!";
          header("Location: ../add-book.php?error=$em");
          exit();
        }
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
