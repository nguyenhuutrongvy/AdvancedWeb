<?php
function get_all_categories()
{
  $curl = curl_init();
  curl_setopt_array($curl, [
    CURLOPT_RETURNTRANSFER => 1,
    CURLOPT_URL => "https://localhost:7115/api/categories",
    CURLOPT_SSL_VERIFYPEER => false,
  ]);

  $resp = curl_exec($curl);

  $result = json_decode($resp, true);

  curl_close($curl);

  return $result;
}

function add_category()
{
  $data = [
    "name" => "Chủ đề cũ nè",
    "urlSlug" => "chu-de-cu-ne",
    "description" => "Chủ đề cũ",
    "showOnMenu" => true,
  ];

  $data_string = json_encode($data, JSON_UNESCAPED_UNICODE);

  $curl = curl_init();

  curl_setopt_array($curl, [
    CURLOPT_URL => "https://localhost:7115/api/categories",
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
}

function update_category($id)
{
  $data = [
    "name" => "Chủ đề này cũ gồi",
    "urlSlug" => "chu-de-nay-cu-goi",
    "description" => "Chủ đề cũ gồi",
    "showOnMenu" => false,
  ];

  $data_string = json_encode($data, JSON_UNESCAPED_UNICODE);

  $curl = curl_init();

  curl_setopt_array($curl, [
    CURLOPT_URL => "https://localhost:7115/api/categories/" . $id,
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
}

function delete_category($id)
{
  $curl = curl_init();

  curl_setopt_array($curl, [
    CURLOPT_URL => "https://localhost:7115/api/categories/" . $id,
    CURLOPT_CUSTOMREQUEST => "DELETE",
    CURLOPT_RETURNTRANSFER => true,
    CURLOPT_SSL_VERIFYPEER => false,
  ]);

  curl_setopt($curl, CURLOPT_HTTPHEADER, ["Content-Type: application/json"]);

  $result = curl_exec($curl);

  curl_close($curl);
}
?>
