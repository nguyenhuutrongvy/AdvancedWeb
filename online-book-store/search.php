<?php
session_start();

# If search key is not set or empty
if (!isset($_GET["key"]) || empty($_GET["key"])) {
  header("Location: index.php");
  exit();
}
$key = $_GET["key"];

# Database Connection File
include "db_conn.php";

# Book helper function
include "php/func-book.php";
$books = search_books($key);

# author helper function
include "php/func-author.php";
$authors = get_all_author();

# Category helper function
include "php/func-category.php";
$categories = get_all_categories();
?>
<!DOCTYPE html>
<html lang="en">
<head>
	<meta charset="UTF-8">
	<meta name="viewport" content="width=device-width, initial-scale=1.0">
	<title>Free Ebooks</title>

    <!-- Bootstrap 5 CDN -->
	<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.1.1/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-F3w7mX95PdgyTmZZMECAngseQB83DfGTowi0iMjiWaeVhAn4FJkqJByhZMI3AhiU" crossorigin="anonymous">

    <!-- Bootstrap 5 JS Bundle CDN -->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.1.1/dist/js/bootstrap.bundle.min.js" integrity="sha384-/bQdsTh/da6pkI1MST/rWKFNjaCP5gBSY4sEBT38Q/9RBh9AH40zEOg7Hlq2THRZ" crossorigin="anonymous"></script>

    <link rel="stylesheet" href="css/style.css">
</head>
<body>
	<div class="container">
		<nav class="navbar navbar-expand-lg navbar-light bg-light">
		  <div class="container-fluid">
		    <a class="navbar-brand" href="index.php">Free Ebooks</a>
		    <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
		      <span class="navbar-toggler-icon"></span>
		    </button>
		    <div class="collapse navbar-collapse" 
		         id="navbarSupportedContent">
		      <ul class="navbar-nav me-auto mb-2 mb-lg-0">
		        <li class="nav-item">
		          <a class="nav-link active" 
		             aria-current="page" 
		             href="index.php">Trang chủ</a>
		        </li>
		        <li class="nav-item">
		          <a class="nav-link" 
		             href="#">Liên hệ</a>
		        </li>
		        <li class="nav-item">
		          <a class="nav-link" 
		             href="#">Giới thiệu</a>
		        </li>
		        <li class="nav-item">
		          <?php if (isset($_SESSION["user_id"])) { ?>
		          	<a class="nav-link" 
		             href="admin.php">Admin</a>
		          <?php } else { ?>
		          <a class="nav-link" 
		             href="login.php">Đăng nhập</a>
		          <?php } ?>
		        </li>
		      </ul>
		    </div>
		  </div>
		</nav><br>

		<h1 class="display-4 p-3 fs-3"> 
			<a href="index.php"
			   class="nd">
				<img src="img/back-arrow.PNG" 
				     width="35">
			</a>
			Kết quả tìm kiếm cho từ khóa <b>"</b><i><?= $key ?></i><b>"</b>
		</h1>

		<div class="d-flex pt-3">
			<?php if (empty($books["result"])) { ?>
				<div class="alert alert-warning 
        	            text-center p-5 pdf-list" 
        	     role="alert">
        	     <img src="img/empty-search.png" 
        	          width="100">
        	     <br>
				  Không tìm thấy sách chứa từ khóa <b>"</b><i><?= $key ?></i><b>"</b>	!
			  </div>
			<?php } else { ?>
			<div class="pdf-list d-flex flex-wrap">
				<?php foreach ($books["result"] as $book) { ?>
				<div class="card m-1">
					<img src="uploads/cover/<?= $book["cover"] ?>"
					     class="card-img-top">
					<div class="card-body">
						<h5 class="card-title">
							<?= $book["title"] ?>
						</h5>
						<p class="card-text">
							<i><b>Tác giả:
								<?php foreach ($authors["result"] as $author) {
          if ($author["id"] == $book["authorId"]) {
            echo $author["name"];
            break;
          } ?>

								<?php
        } ?>
							<br></b></i>
							<?= $book["description"] ?>
							<br><i><b>Chủ đề:
								<?php foreach ($categories["result"] as $category) {
          if ($category["id"] == $book["categoryId"]) {
            echo $category["name"];
            break;
          } ?>

								<?php
        } ?>
							<br></b></i>
						</p>
                       <a href="uploads/files/<?= $book["file"] ?>"
                          class="btn btn-success">Mở</a>

                        <a href="uploads/files/<?= $book["file"] ?>"
                          class="btn btn-primary"
                          download="<?= $book["title"] ?>">Tải</a>
					</div>
				</div>
				<?php } ?>
			</div>
		<?php } ?>
		</div>
	</div>
</body>
</html>