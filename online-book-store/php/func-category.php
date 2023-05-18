<?php

# Get all Categories function
function get_all_categories()
{
  $curl = curl_init();
  curl_setopt_array($curl, [
    CURLOPT_RETURNTRANSFER => 1,
    CURLOPT_URL => "https://localhost:7007/api/categories",
    CURLOPT_SSL_VERIFYPEER => false,
  ]);

  $resp = curl_exec($curl);

  $result = json_decode($resp, true);

  curl_close($curl);

  return $result;
}

# Get category by ID
function get_category($id)
{
  $curl = curl_init();
  curl_setopt_array($curl, [
    CURLOPT_RETURNTRANSFER => 1,
    CURLOPT_URL => "https://localhost:7007/api/categories/" . $id,
    CURLOPT_SSL_VERIFYPEER => false,
  ]);

  $resp = curl_exec($curl);

  $result = json_decode($resp, true);

  curl_close($curl);

  return $result;
}
