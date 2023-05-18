<?php
session_start();

# Database Connection File
include "db_conn.php";

# Book helper function
include "php/func-book.php";
$books = get_all_books();

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
		</nav>
		<form action="search.php"
             method="get" 
             style="width: 100%; max-width: 30rem">

       	<div class="input-group my-5">
		  <input type="text" 
		         class="form-control"
		         name="key" 
		         placeholder="Tên, mô tả sách..." 
		         aria-label="Search Book..." 
		         aria-describedby="basic-addon2">

		  <button class="input-group-text
		                 btn btn-primary" 
		          id="basic-addon2">
		          <img src="img/search.png"
		               width="20">

		  </button>
		</div>
       </form>
		<div class="d-flex pt-3">
			<?php if ($books["isSuccess"] == false) { ?>
				<div class="alert alert-warning 
        	            text-center p-5" 
        	     role="alert">
        	     <img src="img/empty.png" 
        	          width="100">
        	     <br>
			    Không có sách!
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
                          class="btn btn-success">Xem</a>

                        <a href="uploads/files/<?= $book["file"] ?>"
                          class="btn btn-primary"
                          download="<?= $book["title"] ?>">Tải</a>
					</div>
				</div>
				<?php } ?>
			</div>
		<?php } ?>

		<div class="category">
			<!-- List of categories -->
			<div class="list-group">
				<?php if ($categories["isSuccess"] == 0) {
      // do nothing
    } else {
       ?>
				<a href="#"
				   class="list-group-item list-group-item-action active">Chủ đề</a>
				   <?php foreach ($categories["result"] as $category) { ?>
				  
				   <a href="category.php?id=<?= $category["id"] ?>"
				      class="list-group-item list-group-item-action">
				      <?= $category["name"] ?></a>
				<?php }
    } ?>
			</div>

			<!-- List of authors -->
			<div class="list-group mt-5">
				<?php if ($authors["isSuccess"] == false) {
      // do nothing
    } else {
       ?>
				<a href="#"
				   class="list-group-item list-group-item-action active">Tác giả</a>
				   <?php foreach ($authors["result"] as $author) { ?>
				  
				   <a href="author.php?id=<?= $author["id"] ?>"
				      class="list-group-item list-group-item-action">
				      <?= $author["name"] ?></a>
				<?php }
    } ?>
			</div>
		</div>
		</div>
	</div>
</body>
</html>