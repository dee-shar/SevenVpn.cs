#!/bin/bash

sid=null
user_id=null
key=1523615231263112
api="https://panel.7vpn.com/api.cgi"
static_api="https://static.7vpn.com"
user_agent="7vpn android 3.4.0[8830]"

function register() {
	# 1 - login: (string): <login>
	# 2 - password: (string): <password>
	curl --request POST \
		--url "$api/user/registration" \
		--user-agent "$user_agent" \
		--header "content-type: application/json" \
		--header "key: $key" \
		--header "accept-language: en-US" \
		--data '{
			"login": "'$1'",
			"email": "'$1'",
			"password": "'$2'"
		}'
}

function login() {
	# 1 - login: (string): <login>
	# 2 - password: (string): <password>
	response=$(curl --request POST \
		--url "$api/users/login" \
		--user-agent "$user_agent" \
		--header "content-type: application/json" \
		--header "key: $key" \
		--header "accept-language: en-US" \
		--data '{
			"login": "'$1'",
			"password": "'$2'"
		}')
	if [ -n $(jq -r ".sid" <<< "$response") ]; then
		sid=$(jq -r ".sid" <<< "$response")
		user_id=$(jq -r ".uid" <<< "$response")
	fi
	echo $response
}

function get_account_info() {
	curl --request GET \
		--url "$api/user" \
		--user-agent "$user_agent" \
		--header "content-type: application/json" \
		--header "key: $key" \
		--header "accept-language: en-US" \
		--header "usersid: $sid"
}

function get_account_internet() {
	curl --request GET \
		--url "$api/user/internet" \
		--user-agent "$user_agent" \
		--header "content-type: application/json" \
		--header "key: $key" \
		--header "accept-language: en-US" \
		--header "usersid: $sid"
}

function get_servers() {
	curl --request GET \
		--url "$static_api/serverlistplan.json" \
		--user-agent "$user_agent" \
		--header "content-type: application/json"
}

function change_password() {
	# 1 - password: (string): <password>
	curl --request POST \
		--url "$api/user/reset/password/" \
		--user-agent "$user_agent" \
		--header "content-type: application/json" \
		--header "key: $key" \
		--header "accept-language: en-US" \
		--header "usersid: $sid" \
		--data '{
			"password": "'$1'"
		}'
}
