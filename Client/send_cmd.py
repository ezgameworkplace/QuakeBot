# -*- coding: utf-8 -*-
"""
File:send_cmd.py
Author:ezgameworkplace
Date:2023/6/9
"""
import json
import socket
import time
from typing import Dict


def send_command_to_unity(command:Dict):
    # 创建一个socket对象
    s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

    # 连接到你的Unity服务器，这里假设服务器运行在本地机器上的9999端口
    s.connect(('localhost', 9999))

    # 发送你的命令, 将字典转换为JSON字符串
    json_str = json.dumps(command)
    s.sendall(json_str.encode('utf-8'))

    # 接收服务器的响应
    data = s.recv(1024)
    print("Received: " + data.decode('utf-8'))
    # 关闭连接
    s.close()


if __name__ == '__main__':

    # 用你的命令替换 "your_command_here"
    dict_cmd = {
        "command": "ClickButton",
        "parameters": "Talk"
    }
    while True:
        time.sleep(0.5)
        send_command_to_unity(dict_cmd)
