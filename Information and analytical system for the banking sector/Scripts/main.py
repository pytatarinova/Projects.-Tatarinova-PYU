#!/usr/bin/env python3
# -*- coding: utf-8 -*-
"""
Приложение для анализа данных кредитных историй заёмщиков
"""
import sys
import os
import scripts
import numpy as np


ABS_PATH = os.path.abspath('main.py')
if '\\' in ABS_PATH:
    ABS_PATH = '\\'.join(ABS_PATH.split('\\')[:len(ABS_PATH.split('\\')) - 2])
else:
    ABS_PATH = '/'.join(ABS_PATH.split('/')[:len(ABS_PATH.split('/')) - 2])
sys.path.append(ABS_PATH)


import Library.libraries


if '\\' in ABS_PATH:
    FILE = ABS_PATH + '\\Data\\Base_edited.xlsx'  # файл с исходной базой данных
else:
    FILE = ABS_PATH + '/Data/Base_edited.xlsx'
data = Library.libraries.read_from_text_file(FILE)  # чтение файла
data_columns = data.columns  # получение названий столбцов базы данных
columns = []
int_columns = []
string_columns = []
float_columns = []
for i, cols in enumerate(data_columns):
    columns.append(cols)  # добавление названий столбцов в список
    if isinstance(data.iloc[1, i], str):  # если тип данных в столбце str
        string_columns.append(cols)  # добавление названия столбца в список переменных
        # типа str
    elif isinstance(data.iloc[1, i], np.float64):  # если тип данных в столбце float
        float_columns.append(cols)  # добавление названия столбца в список переменных
        # типа float
    else:  # если тип данных в столбце int
        int_columns.append(cols)  # добавление названия столбца в список переменных типа int
qualitative_variables = ['Attrition_Flag', 'Gender', 'Education_Level',
                         'Marital_Status', 'Income_Category', 'Card_Category']  # список
# качественных переменных
quantitative_variables = ['Customer_Age', 'Dependent_count', 'Months_on_book',
                          'Total_Relationship_Count', 'Months_Inactive_12_mon',
                          'Contacts_Count_12_mon', 'Credit_Limit',
                          'Total_Revolving_Bal', 'Avg_Open_To_Buy',
                          'Total_Amt_Chng_Q4_Q1', 'Total_Trans_Amt',
                          'Total_Trans_Ct', 'Total_Ct_Chng_Q4_Q1',
                          'Avg_Utilization_Ratio']  # список количественных переменных
scripts.interface(columns, data, qualitative_variables, quantitative_variables, string_columns,
                  int_columns, float_columns)  # вызов и запуск интерфейса
