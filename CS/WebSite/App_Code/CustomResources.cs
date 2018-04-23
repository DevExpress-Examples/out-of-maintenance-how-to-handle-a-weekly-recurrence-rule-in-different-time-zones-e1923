using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

[Serializable]
public class CustomResourceList : BindingList<CustomResource> {		
	public void AddRange(CustomResourceList resources) {
		foreach (CustomResource customResource in resources)
			this.Add(customResource);
	}
	public int GetResourceIndex(object resourceId) {
		for (int i = 0; i < Count; i++)
			if (this[i].Id == resourceId)
				return i;
		return -1;
	}
}
public class CustomResourceDataSource {
	CustomResourceList resources;
	public CustomResourceDataSource(CustomResourceList resources) {
		if (resources == null)
			DevExpress.XtraScheduler.Native.Exceptions.ThrowArgumentNullException("resources");
		this.resources = resources;
	}
	public CustomResourceDataSource()
		: this(new CustomResourceList()) {
	}
	public CustomResourceList Resources { get { return resources; } set { resources = value; } }

	#region ObjectDataSource methods
	public void InsertMethodHandler(CustomResource customResource) {
		Resources.Add(customResource);
	}
	public void DeleteMethodHandler(CustomResource customResource) {
		int resourceIndex = Resources.GetResourceIndex(customResource.Id);
		if (resourceIndex >= 0)
			Resources.RemoveAt(resourceIndex);
	}
	public void UpdateMethodHandler(CustomResource customResource) {
		int resourceIndex = Resources.GetResourceIndex(customResource.Id);
		if (resourceIndex >= 0) {
			Resources.RemoveAt(resourceIndex);
			Resources.Insert(resourceIndex, customResource);
		}
	}
	public IEnumerable SelectMethodHandler() {
		CustomResourceList result = new CustomResourceList();
		result.AddRange(Resources);
		return result;
	}
	#endregion
}

[Serializable]
public class CustomResource {
	object id;
	string caption;

	public CustomResource() {
	}

	public object Id { get { return id; } set { id = value; } }
	public string Caption { get { return caption; } set { caption = value; } }
}