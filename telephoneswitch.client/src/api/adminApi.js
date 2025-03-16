export const getUsers = async (accessToken) => {
  const res = await fetch("http://localhost:5204/api/admin/get-users", {
    headers: {
      Authorization: `Bearer ${accessToken}`,
    },
  });

  if (!res.ok) {
    throw new Error("Failed to fetch users");
  }

  return res.json();
};

export const getUnpaidBills = async (accessToken) => {
  const res = await fetch("http://localhost:5204/api/admin/unpaid-bills", {
    headers: {
      Authorization: `Bearer ${accessToken}`,
    },
  });

  if (!res.ok) {
    throw new Error("Failed to fetch unpaid bills");
  }

  return res.json();
};

export const blockUser = async (userId, accessToken) => {
  const res = await fetch(`http://localhost:5204/api/admin/block-user/${userId}`, {
    method: "POST",
    headers: {
      Authorization: `Bearer ${accessToken}`,
    },
  });

  if (!res.ok) {
    throw new Error("Failed to block user");
  }
};

export const unblockUser = async (userId, accessToken) => {
  const res = await fetch(`http://localhost:5204/api/admin/unblock-user/${userId}`, {
    method: "POST",
    headers: {
      Authorization: `Bearer ${accessToken}`,
    },
  });

  if (!res.ok) {
    throw new Error("Failed to unblock user");
  }
};

export const addUser = async (name, phoneNumber, accessToken) => {
  const res = await fetch(`http://localhost:5204/api/admin/add-user/${name}/${phoneNumber}`, {
    method: "POST",
    headers: {
      Authorization: `Bearer ${accessToken}`,
    },
  });

  if (!res.ok) {
    throw new Error("Error when adding the subscriber");
  }

  return res.json();
};
