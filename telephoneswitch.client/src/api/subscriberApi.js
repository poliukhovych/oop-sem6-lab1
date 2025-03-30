export const getAvailableServices = async (accessToken) => {
  const res = await fetch("http://localhost:5204/api/subscriber/services", {
    headers: {
      Authorization: `Bearer ${accessToken}`,
    },
  });

  if (!res.ok) {
    throw new Error("Error fetching available services");
  }

  return res.json();
};

export const getMyServices = async (accessToken) => {
  const res = await fetch("http://localhost:5204/api/subscriber/my-services", {
    headers: {
      Authorization: `Bearer ${accessToken}`,
    },
  });

  if (!res.ok) {
    throw new Error("Error fetching your services");
  }

  return res.json();
};

export const addService = async (serviceId, accessToken) => {
  const res = await fetch(`http://localhost:5204/api/subscriber/add-service/${serviceId}`, {
    method: "POST",
    headers: {
      Authorization: `Bearer ${accessToken}`,
    },
  });

  if (!res.ok) {
    throw new Error("Error when activating service");
  }
};

export const getMyBills = async (accessToken) => {
  const res = await fetch("http://localhost:5204/api/subscriber/my-bills", {
    headers: {
      Authorization: `Bearer ${accessToken}`,
    },
  });

  if (!res.ok) {
    throw new Error("Error fetching bills");
  }

  return res.json();
};

export const payBill = async (billId, accessToken) => {
  const res = await fetch(`http://localhost:5204/api/subscriber/pay-bill/${billId}`, {
    method: "POST",
    headers: {
      Authorization: `Bearer ${accessToken}`,
    },
  });

  if (!res.ok) {
    throw new Error("Error when paying bill");
  }
};
