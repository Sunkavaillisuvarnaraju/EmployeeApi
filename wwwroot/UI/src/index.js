async function onbodyLoad() {
  try {
    const response = await fetch(" https://localhost:7162/api/employee");

    if (!response.ok) {
      throw new Error(`HTTP error! status: ${response.status}`);
    }

    const data = await response.json(); // Assuming your API returns a JSON array

    const tbody = document.getElementById("tableBody");
    tbody.innerHTML = "";

    data.forEach(emp => {
      const row = document.createElement("tr");

      row.innerHTML = `
          <td>${emp.id}</td>
          <td>${emp.name}</td>
          <td>${emp.salary}</td>
          <td class="text-center">
          <div class="d-inline-flex gap-2">
          <button class="btn btn-primary btn-sm"
              data-bs-toggle="modal"
              data-bs-target="#editModal"
              onclick='openEditModal(${JSON.stringify(emp)})'>
        Edit
      </button>
      <button class="btn btn-danger btn-sm">Delete</button>
    </div>
  </td>          
        `;

      tbody.appendChild(row);
    });

  } catch (error) {
    console.error("Failed to fetch employee data:", error);
  }
}

async function saveEmployee() {
  const name = document.getElementById("empName").value;
  const salary = parseFloat(document.getElementById("empSalary").value);

  const employee = {
    name: name,
    salary: salary
  };

  try {
    const response = await fetch("https://localhost:7162/api/employee", {
      method: "POST",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify(employee)
    });

    if (response.ok) {
      alert("Employee saved successfully!");
      onbodyLoad();
    } else {
      alert("Failed to save employee");
    }
  } catch (error) {
    console.error("Error saving employee:", error);
    alert("Error occurred while saving.");
  }
}

async function updateEmployee() {
  const id = document.getElementById("editEmpId").value;
  const name = document.getElementById("editEmpName").value;
  const salary = parseFloat(document.getElementById("editEmpSalary").value);

  const updatedEmployee = { id, name, salary };

  const response = await fetch(`https://localhost:7162/api/employee`, {
    method: "PUT",
    headers: {
      "Content-Type": "application/json"
    },
    body: JSON.stringify(updatedEmployee)
  });

  if (response.ok) {
    alert("Updated successfully!");
    onbodyLoad(); // refresh the list
    const modal = bootstrap.Modal.getInstance(document.getElementById("editModal"));
    modal.hide();
  } else {
    alert("Update failed!");
  }
}
function openEditModal(emp) {
  document.getElementById("editEmpId").value = emp.id;
  document.getElementById("editEmpName").value = emp.name;
  document.getElementById("editEmpSalary").value = emp.salary;
}
