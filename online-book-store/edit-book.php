<?php
session_start();

# If the admin is logged in
if (isset($_SESSION["user_id"]) && isset($_SESSION["user_email"])) {

  # If book ID is not set
  if (!isset($_GET["id"])) {
    #Redirect to admin.php page
    header("Location: admin.php");
    exit();
  }

  $id = $_GET["id"];

  # Database Connection File
  include "db_conn.php";

  # Book helper function
  include "php/func-book.php";
  $book = get_book($id);

  # If the ID is invalid
  if ($book["isSuccess"] == false) {
    #Redirect to admin.php page
    header("Location: admin.php");
    exit();
  }

  # Category helper function
  include "php/func-category.php";
  $categories = get_all_categories();

  # author helper function
  include "php/func-author.php";
  $authors = get_all_author();
  ?>

<!DOCTYPE html>
<html lang="en">
<head>
	<meta charset="UTF-8">
	<meta name="viewport" content="width=device-width, initial-scale=1.0">
	<title>Sửa thông tin sách</title>

    <!-- Bootstrap 5 CDN-->
	<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.1.1/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-F3w7mX95PdgyTmZZMECAngseQB83DfGTowi0iMjiWaeVhAn4FJkqJByhZMI3AhiU" crossorigin="anonymous">

    <!-- Bootstrap 5 JS Bundle CDN-->
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.1.1/dist/js/bootstrap.bundle.min.js" integrity="sha384-/bQdsTh/da6pkI1MST/rWKFNjaCP5gBSY4sEBT38Q/9RBh9AH40zEOg7Hlq2THRZ" crossorigin="anonymous"></script>
</head>
<body>
	<div class="container">
		<nav class="navbar navbar-expand-lg navbar-light bg-light">
		  <div class="container-fluid">
		    <a class="navbar-brand" href="admin.php">Admin</a>
		    <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
		      <span class="navbar-toggler-icon"></span>
		    </button>
		    <div class="collapse navbar-collapse" 
		         id="navbarSupportedContent">
		      <ul class="navbar-nav me-auto mb-2 mb-lg-0">
		        <li class="nav-item">
		          <a class="nav-link" 
		             aria-current="page" 
		             href="index.php">Trang chủ</a>
		        </li>
		        <li class="nav-item">
		          <a class="nav-link" 
		             href="add-book.php">Thêm sách</a>
		        </li>
		        <li class="nav-item">
		          <a class="nav-link" 
		             href="add-category.php">Thêm chủ đề</a>
		        </li>
		        <li class="nav-item">
		          <a class="nav-link" 
		             href="add-author.php">Thêm tác giả</a>
		        </li>
		        <li class="nav-item">
		          <a class="nav-link" 
		             href="logout.php">Đăng xuất</a>
		        </li>
		      </ul>
		    </div>
		  </div>
		</nav>
     <form action="php/edit-book.php"
           method="post"
           enctype="multipart/form-data" 
           class="shadow p-4 rounded mt-5"
           style="width: 90%; max-width: 50rem;">

     	<h1 class="text-center pb-5 display-4 fs-3">
     		Sửa thông tin sách
     	</h1>
     	<?php if (isset($_GET["error"])) { ?>
          <div class="alert alert-danger" role="alert">
			  <?= htmlspecialchars($_GET["error"]) ?>
		  </div>
		<?php } ?>
		<?php if (isset($_GET["success"])) { ?>
          <div class="alert alert-success" role="alert">
			  <?= htmlspecialchars($_GET["success"]) ?>
		  </div>
		<?php } ?>
     	<div class="mb-3">
		    <label class="form-label">
		           Tiêu đề
		           </label>
		    <input type="text" 
		           hidden
		           value="<?= $book["result"]["id"] ?>" 
		           name="book_id">

		    <input type="text" 
		           class="form-control"
		           value="<?= $book["result"]["title"] ?>" 
		           name="book_title">
		</div>

		<div class="mb-3">
		    <label class="form-label">
		           Miêu tả
		           </label>
		    <input type="text" 
		           class="form-control" 
		           value="<?= $book["result"]["description"] ?>"
		           name="book_description">
		</div>

		<div class="mb-3">
		    <label class="form-label">
		           Tác giả
		           </label>
		    <select name="book_author"
		            class="form-control">
		    	    <option value="0">
		    	    	- Chọn tác giả -
		    	    </option>
		    	    <?php if ($authors["isSuccess"] == false) {
             # Do nothing!
           } else {
             foreach ($authors["result"] as $author) {
               if ($book["result"]["authorId"] == $author["id"]) { ?>
		    	    	<option 
		    	    	  selected
		    	    	  value="<?= $author["id"] ?>">
		    	    	  <?= $author["name"] ?>
		    	        </option>
		    	        <?php } else { ?>
						<option 
							value="<?= $author["id"] ?>">
							<?= $author["name"] ?>
						</option>
		    	   <?php }
             }
           } ?>
		    </select>
		</div>

		<div class="mb-3">
		    <label class="form-label">
		           Chủ đề
		           </label>
		    <select name="book_category"
		            class="form-control">
		    	    <option value="0">
		    	    	- Chọn chủ đề -
		    	    </option>
		    	    <?php if ($categories["isSuccess"] == false) {
             # Do nothing!
           } else {
             foreach ($categories["result"] as $category) {
               if ($book["result"]["categoryId"] == $category["id"]) { ?>
		    	    	<option 
		    	    	  selected
		    	    	  value="<?= $category["id"] ?>">
		    	    	  <?= $category["name"] ?>
		    	        </option>
		    	        <?php } else { ?>
						<option 
							value="<?= $category["id"] ?>">
							<?= $category["name"] ?>
						</option>
		    	   <?php }
             }
           } ?>
		    </select>
		</div>

		<div class="mb-3">
		    <label class="form-label">
		           Hình thu nhỏ
		           </label>
		    <input type="file" 
		           class="form-control" 
		           name="book_cover">

		     <input type="text" 
		           hidden
		           value="<?= $book["result"]["cover"] ?>" 
		           name="current_cover">

		    <a href="uploads/cover/<?= $book["result"]["cover"] ?>"
		       class="link-dark">Hình hiện tại</a>
		</div>

		<div class="mb-3">
		    <label class="form-label">
		           File
		           </label>
		    <input type="file" 
		           class="form-control" 
		           name="file">

		     <input type="text" 
		           hidden
		           value="<?= $book["result"]["file"] ?>" 
		           name="current_file">

		    <a href="uploads/files/<?= $book["result"]["file"] ?>"
		       class="link-dark">File hiện tại</a>
		</div>

	    <button type="submit" 
	            class="btn btn-primary">
	            Cập nhật</button>
     </form>
	</div>
</body>
</html>

<?php
} else {
  header("Location: login.php");
  exit();
} ?>
