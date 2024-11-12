function updateCharCount(id) {
    const input = document.getElementById(id);
    const charCount = document.getElementById('charCount' + id.slice(-1));
    charCount.textContent = `${input.value.length}/50`;
}

function generateList() {
    const groceries = [];
    for (let i = 1; i <= 7; i++) {
        const grocery = document.getElementById('grocery' + i).value.trim();
        if (grocery === '') {
            document.getElementById('grocery' + i).classList.add('error');
        } else {
            document.getElementById('grocery' + i).classList.remove('error');
            groceries.push(grocery);
        }
    }

    if (groceries.length > 0) {
        document.getElementById('completionMessage').style.display = 'block';
        setTimeout(() => {
            document.getElementById('completionMessage').style.display = 'none';
        }, 3000);
        alert('Your grocery list: ' + groceries.join(', '));
    } else {
        alert('Please fill in the grocery fields.');
    }
}

function clearFields() {
    for (let i = 1; i <= 7; i++) {
        document.getElementById('grocery' + i).value = '';
        document.getElementById('grocery' + i).classList.remove('error');
        document.getElementById('charCount' + i).textContent = '0/50';
    }
}

for (let i = 1; i <= 7; i++) {
    document.getElementById('grocery' + i).addEventListener('input', () => updateCharCount('grocery' + i));
}


function toggleSidebar() {
    const sidebar = document.getElementById("sidebar");
    const openBtn = document.querySelector(".openbtn");

    if (sidebar.style.width === "250px") {
        sidebar.style.width = "0";  // Close the sidebar
        openBtn.style.display = "block";  // Show the open button again
    } else {
        sidebar.style.width = "250px";  // Open the sidebar
        openBtn.style.display = "none";  // Hide the open button
    }
}
