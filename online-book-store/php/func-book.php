<?php

# Get All books function
function get_all_books()
{
  $curl = curl_init();
  curl_setopt_array($curl, [
    CURLOPT_RETURNTRANSFER => 1,
    CURLOPT_URL => "https://localhost:7007/api/books",
    CURLOPT_SSL_VERIFYPEER => false,
  ]);

  $resp = curl_exec($curl);

  $result = json_decode($resp, true);

  curl_close($curl);

  return $result;
}

# Get  book by ID function
function get_book($id)
{
  $curl = curl_init();
  curl_setopt_array($curl, [
    CURLOPT_RETURNTRANSFER => 1,
    CURLOPT_URL => "https://localhost:7007/api/books/" . $id,
    CURLOPT_SSL_VERIFYPEER => false,
  ]);

  $resp = curl_exec($curl);

  $result = json_decode($resp, true);

  curl_close($curl);

  return $result;
}

# Search books function
function search_books($key)
{
  $curl = curl_init();
  curl_setopt_array($curl, [
    CURLOPT_RETURNTRANSFER => 1,
    CURLOPT_URL => "https://localhost:7007/api/books?Keyword=" . $key,
    CURLOPT_SSL_VERIFYPEER => false,
  ]);

  $resp = curl_exec($curl);

  $result = json_decode($resp, true);

  curl_close($curl);

  return $result;
}

# get books by category
function get_books_by_category($id)
{
  $curl = curl_init();
  curl_setopt_array($curl, [
    CURLOPT_RETURNTRANSFER => 1,
    CURLOPT_URL => "https://localhost:7007/api/categories/" . $id . "/books",
    CURLOPT_SSL_VERIFYPEER => false,
  ]);

  $resp = curl_exec($curl);

  $result = json_decode($resp, true);

  curl_close($curl);

  return $result;
}

# get books by author
function get_books_by_author($id)
{
  $curl = curl_init();
  curl_setopt_array($curl, [
    CURLOPT_RETURNTRANSFER => 1,
    CURLOPT_URL => "https://localhost:7007/api/authors/" . $id . "/books",
    CURLOPT_SSL_VERIFYPEER => false,
  ]);

  $resp = curl_exec($curl);

  $result = json_decode($resp, true);

  curl_close($curl);

  return $result;
}
