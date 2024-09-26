#!/bin/sh
set -e

CURRENT_DIRECTORY="$(dirname "$0")"

cd ${CURRENT_DIRECTORY}/../

echo ""
echo "Create python version environment.."

python -m venv "./.venv"

VENV_PYTHON="./.venv/Scripts/python.exe"

echo ""
echo "Install requirement packages... (${VENV_PYTHON})"

${VENV_PYTHON} -m pip install -r ./requirements.txt 